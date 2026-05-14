using Microsoft.EntityFrameworkCore;
using MyApi.Interfaces;
using MyApi.Models;
using MyApi.DTOs;

namespace MyApi.Repositories;

public class MessageRepository : IMessageRepository
{
  private readonly TestContext _context;
  public MessageRepository(TestContext context)
  {
    _context = context;
  }
  
  public async Task<long> GetOrCreateConversation(long userId)
  {
    var count = await _context.UserCustomers
      .Where(x => (x.Id == userId || x.Id == 44) && x.DeletedAt == null)
      .CountAsync();
    if(count != 2)
      throw new Exception("Khong ton tai nguoi dung");
    var conversation = await _context.MessageConversations
      .Where(c => c.MessageConversationMembers.Count == 2)
      .FirstOrDefaultAsync(c =>
        c.MessageConversationMembers.Any(u => u.UserId == 44) &&
        c.MessageConversationMembers.Any(u => u.UserId == userId)
      );

    if (conversation != null)
      return conversation.Id;
    var newConversation = new MessageConversation
    {
      Type = 1,
      CreatedAt = DateTime.Now,
    };
    _context.MessageConversations.Add(newConversation);
    await _context.SaveChangesAsync();
    var newConversationMemeber = new MessageConversationMember
    {
      UserId = userId,
      ConversationId = newConversation.Id,
      CreatedAt = DateTime.Now,
    };
    var newConersationAdmin = new MessageConversationMember
    {
      UserId = 44,
      ConversationId = newConversation.Id,
      CreatedAt = DateTime.Now
    };
    _context.MessageConversationMembers.Add(newConversationMemeber);
    _context.MessageConversationMembers.Add(newConersationAdmin);
    await _context.SaveChangesAsync();
    return newConversation.Id;
  }

  public async Task<List<MessageGetResDto>?> ListMessage(long conversationId)
  {
    var messages = await _context.MessageMessages
      .Where(x => x.ConversationId == conversationId && x.DeletedAt == null)
      .OrderByDescending(x => x.CreatedAt)
      .Take(100)
      .Select(x => new MessageGetResDto
      {
        MessageId = x.Id,
        Message = x.Message,
        CreatedAt = x.CreatedAt,
        SenderId = x.SenderId,
      })
      .OrderBy(x => x.CreatedAt)
      .ToListAsync();
      return messages;
  }
  public async Task<MessageMessage> SaveMessage(long senderId, long conversationId, string message)
  {
    var newMessage = new MessageMessage
    {
      SenderId = senderId,
      Message = message,
      CreatedAt = DateTime.Now,
      ConversationId = conversationId,
      Type = 1,
    };
    _context.MessageMessages.Add(newMessage);
    await _context.SaveChangesAsync();
    return newMessage;
  }
  public async Task<List<ContactGetResDto>?> ListContact(long userId)
  {
    var contacts = await _context.MessageConversationMembers
      .Where(x => x.UserId == userId && x.DeletedAt == null)
      .Select(x => new ContactGetResDto
      {
        ConversationId = x.ConversationId,
        NameContact = _context.MessageConversationMembers
          .Where(y => y.UserId != userId && y.DeletedAt == null && y.ConversationId == x.ConversationId)
          .Select(y => y.User.Name)
          .FirstOrDefault() ?? "",
        PreviewMessage = x.Conversation.MessageMessages.OrderByDescending(y => y.CreatedAt)
          .Select(y => new PreviewMessageResDto
          {
            LastMessageId = y.Id,
            LastMessage = y.Message,
            SenderId = y.SenderId,
            SenderName = y.Sender.Name,
            TimeLastMessage = y.CreatedAt,
            IsSeen = x.LastSeenMessage == y.Id,
          })
          .FirstOrDefault(),
      })
      .ToListAsync();
    return contacts;
  }
  public async Task MarkAsRead(long userId, long conversationId, long messageId)
  {
    var member = await _context.MessageConversationMembers
      .Where(x => x.UserId == userId && x.ConversationId == conversationId && x.DeletedAt == null)
      .Select(x => x)
      .FirstOrDefaultAsync();
    Console.WriteLine($"Tim duoc member ko vay {member}");
    if(member == null)
      return;
    member.LastSeenMessage = messageId;
    await _context.SaveChangesAsync();
  }
}