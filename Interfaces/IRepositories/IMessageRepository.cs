using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Interfaces;

public interface IMessageRepository
{
  public Task<long> GetOrCreateConversation(long userId);
  public Task<List<MessageGetResDto>?> ListMessage(long conversationId);
  public Task<MessageMessage> SaveMessage(long userId, long conversationId, string message);
  public Task<List<ContactGetResDto>?> ListContact(long userId);
}