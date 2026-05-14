using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Interfaces;
public interface IMessageService : IServiceScoped
{
  public Task<long> GetOrCreateConversation(long userId);
  public Task<List<MessageGetResDto>?> ListMessage(long conversationId);
  public Task<MessageMessage> SaveMessage(long senderId, long conversationId, string message);
  public Task<List<ContactGetResDto>?> ListContact(long userId);
  public Task MarkAsRead(long userId, long conversationId, long messageId);
}