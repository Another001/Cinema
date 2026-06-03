using Microsoft.EntityFrameworkCore;
using MyApi.Interfaces;
using MyApi.Models;
using MyApi.DTOs;
using MyApi.Repositories;
using System.Linq.Expressions;

namespace MyApi.Services;

public class MessageService : IMessageService
{
  private readonly IMessageRepository _messageRepo;
  public MessageService(IMessageRepository messageRepo)
  {
    _messageRepo = messageRepo;
  }
  public async Task<long> GetOrCreateConversation(long userId)
  {
    var conversationId = await _messageRepo.GetOrCreateConversation(userId);
    return conversationId;
  }
  public async Task<List<MessageGetResDto>?> ListMessage(long conversationId)
  {
    var messages = await _messageRepo.ListMessage(conversationId);
      return messages;
  }
  public async Task<List<MessageGetResDto>?> ListMessage_v2(long userId)
  {
    var messages = await _messageRepo.ListMessage_v2(userId);
    return messages;
  }
  public async Task<MessageMessage> SaveMessage(long senderId, long conversationId, string message)
  {
    var msg =  await _messageRepo.SaveMessage(senderId, conversationId, message);
    return msg;
  }
  public async Task<List<ContactGetResDto>?> ListContact(long userId)
  {
    var contacts = await _messageRepo.ListContact(userId);
    return contacts;
  }
  public async Task<List<ContactGetResDto>?> ListContact_v2(long userId, string conversationState)
  {
    var contact = await _messageRepo.ListContact_v2(userId, conversationState);
    return contact;
  }
  public async Task<List<MessageGetResDto>?> ListMessageBySupport_v2(long conversationId)
  {
    var messages = await _messageRepo.ListMessageBySupport_v2(conversationId);
    return messages;
  }
  public async Task MarkAsRead(long userId, long conversationId, long messageId)
  {
    await _messageRepo.MarkAsRead(userId, conversationId, messageId);
    return;
  }
  public async Task<StateConversationDTO> SaveConversationState(long conversationId, string role)
  {
    var state = await _messageRepo.SaveConversationState(conversationId, role);
    return state;
  }
  public async Task<long> CreateConversationByUser(long userId)
  {
    var conversationId = await _messageRepo.CreateConversationByUser(userId);
    return conversationId;
  }
  public async Task AddSupportToConversation(long conversationId, long userId)
  {
    await _messageRepo.AddSupportToConversation(conversationId, userId);
    return;
  }
  public async Task MarkConversationDone(long conversationId)
  {
    await _messageRepo.MarkConversationDone(conversationId);
    return;
  }
}