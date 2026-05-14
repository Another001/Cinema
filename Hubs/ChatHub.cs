using Microsoft.AspNetCore.SignalR;
using MyApi.Interfaces;

public class ChatHub : Hub
{
  private readonly IMessageService _messageService;
  public ChatHub(IMessageService messageService)
  {
    _messageService = messageService;
  }
  public async Task JoinConversation(long conversationId)
  {
    await Groups.AddToGroupAsync(
      Context.ConnectionId,
      $"conversation-{conversationId}"
    );
  }
  public async Task SendMessage(long senderId, long conversationId, string message)
  {
    var msg = await _messageService.SaveMessage(senderId, conversationId, message);
    Console.WriteLine("Nhan duoc message moi");
    await Clients.Group($"conversation-{conversationId}")
      .SendAsync("ReceiveMessage", senderId, message, msg.Id, msg.CreatedAt);
    Console.WriteLine($"Gui cho user ne ${senderId} ${conversationId} ${message}");
    await Clients.Group("userId-44").SendAsync("UpdateChatList",senderId, conversationId, message, msg.Id, msg.CreatedAt);
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
    if(userId != 44)
      return;
    await Groups.AddToGroupAsync(Context.ConnectionId ,$"userId-44");
  }
  public async Task MaskAsRead(long userId, long conversationId, long messageId)
  {
    await _messageService.MarkAsRead(userId, conversationId, messageId);
  }
}