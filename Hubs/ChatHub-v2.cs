using Microsoft.AspNetCore.SignalR;
using MyApi.Interfaces;

public class ChatHub_v2 : Hub
{
  private readonly ICustomerService _customerService;
  private readonly IMessageService _messageService;
  public ChatHub_v2(ICustomerService customerService, IMessageService messageService)
  {
    _customerService = customerService;
    _messageService = messageService;
  }
  public async Task JoinConversation(long conversationId)
  {
    await Groups.AddToGroupAsync(Context.ConnectionId, $"conversation-{conversationId}");
  }
  public async Task SendMessage(long senderId, long conversationId, string message)
  {
    var user = await _customerService.Get(senderId);
    if(user == null)
      throw new Exception("Khong tim thay nguoi dung");
    var msg = await _messageService.SaveMessage(senderId, conversationId, message);
    if(msg == null)
      throw new Exception("Khong the gui tin nhan");
    var stateConversation = await _messageService.SaveConversationState(conversationId, user.Role);
    Console.WriteLine($"supporter id la {stateConversation.SupporterId}");
    await Clients.Group($"conversation-{conversationId}")
      .SendAsync("ReceiveMessage", user.Name, senderId, message, msg.Id, msg.CreatedAt);
    var payload = new
      {
        senderName = user.Name,
        senderId = senderId,
        conversationId = conversationId,
        message = message,
        msgId = msg.Id,
        senderPhone = user.Phone,
        msgCreatedAt = msg.CreatedAt
      };
      Console.WriteLine($"name payloaf la {payload.senderName} {senderId} {payload.message}");
    if(stateConversation.State.Trim() == "active")
    {
      Console.WriteLine($"Truong hop active kich hoat Tin nhan gui la {message} id conversation la {conversationId} state la {stateConversation.State}");
      await Clients.Group($"supporter-{stateConversation.SupporterId}")
        .SendAsync("UpdateActive", payload);
    }
    else
    {
      Console.WriteLine($"Truong hop non active kich hoat Tin nhan gui la {message} id conversation la {conversationId} state la {stateConversation.State}");
      await Clients.Group("supporter-waiting")
        .SendAsync("UpdateWaiting", payload);
    }
  }
  public async Task LeaveConversation(long conversationId)
  {
    await Groups.RemoveFromGroupAsync(
      Context.ConnectionId,
      $"conversation-{conversationId}"
    );
  }
  public async Task UserInit(long userId)
  {
    await Groups.AddToGroupAsync(Context.ConnectionId ,$"supporter-waiting");
    await Groups.AddToGroupAsync(Context.ConnectionId, $"supporter-{userId}");
  }
  public async Task MoveConversationFromPool(long conversationId, long supporterId)
  {
    await Clients.Group("supporter-waiting")
      .SendAsync("DeletedWaitingConversation", conversationId);
    await Clients.Group($"supporter-{supporterId}")
      .SendAsync("AddActiveConversation", conversationId);
  }
  public async Task MarkConversationDone(long conversationId, long supporterId)
  {
    await _messageService.MarkConversationDone(conversationId);
    await Clients.Group($"conversation-{conversationId}")
      .SendAsync("DeletedActiveConversation", conversationId);
  }
}