using Microsoft.EntityFrameworkCore;
using MyApi.Interfaces;
using MyApi.Models;
using MyApi.DTOs;
using MyApi.Repositories;

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
}