using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Interfaces;
public interface IMessageService : IServiceScoped
{
  public Task<long> GetOrCreateConversation(long userId);
  public Task<List<MessageGetResDto>?> ListMessage(long conversationId);
  public Task<List<MessageGetResDto>?> ListMessage_v2(long userId);
  public Task<MessageMessage> SaveMessage(long senderId, long conversationId, string message);
  public Task<List<ContactGetResDto>?> ListContact(long userId);
  public Task<List<ContactGetResDto>?> ListContact_v2(long userId, string conversationState);
  public Task MarkAsRead(long userId, long conversationId, long messageId);
  public Task<StateConversationDTO> SaveConversationState(long conversationId, string role);
  public Task<long> CreateConversationByUser(long userId);
  public Task<List<MessageGetResDto>?> ListMessageBySupport_v2(long conversationId);
  public Task AddSupportToConversation(long conversationId, long userId);
  public Task MarkConversationDone(long conversationId);
}