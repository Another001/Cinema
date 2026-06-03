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

  public async Task<long> CreateConversationByUser(long userId)
  {
    var user = await _context.UserCustomers
      .Where(x => x.Id == userId && x.DeletedAt == null)
      .FirstOrDefaultAsync();
    if(user == null)
      throw new Exception("Khong ton tai nguoi dung");
    var conversationId = await _context.MessageConversationMembers
      .Where(x => x.UserId == userId && x.DeletedAt == null
      && (x.Conversation.State == "waiting" || x.Conversation.State == "active")
      && x.Conversation.CreatedAt > DateTime.Now.AddMinutes(-30) && x.Conversation.DeletedAt == null)
      .Select(x => x.ConversationId)
      .FirstOrDefaultAsync();
    Console.WriteLine($"lay con verdutn id la {conversationId}");
    if(conversationId != 0)
    {
      return conversationId;
    }
    var newConversation = new MessageConversation
    {
      Type = 1,
      Name = user.Name,
      CreatedAt = DateTime.Now,
      State = "waiting",
    };

    _context.MessageConversations.Add(newConversation);
    await _context.SaveChangesAsync();
    var member = new MessageConversationMember
    {
      UserId = userId,
      ConversationId = newConversation.Id,
      CreatedAt = DateTime.Now,
    };
    _context.MessageConversationMembers.Add(member);
    await _context.SaveChangesAsync();
    return newConversation.Id;
  }

  public async Task CreateConversationMember(long conversationId, long userId)
  {
    var supporter = await _context.UserCustomers
      .Where(x => x.Id == userId && x.DeletedAt == null)
      .Include(x => x.UserType)
      .FirstOrDefaultAsync();
    if(supporter == null || supporter?.UserType.Code != "Support" || supporter?.UserType.Code != "Admin")
    {
      throw new Exception("Khong tim thay ho tro vien");
    }
    var conversation = await _context.MessageConversations
      .Where(x => x.Id == conversationId && x.DeletedAt == null && x.CreatedAt > DateTime.Now.AddMinutes(-30))
      .FirstOrDefaultAsync();
    if(conversation == null)
    {
      throw new Exception("Phien tra loi het han hoac khong ton tai");
    }
    var supportMember = new MessageConversationMember
    {
      UserId = userId,
      ConversationId = conversationId,
      CreatedAt = DateTime.Now,
    };
    _context.MessageConversationMembers.Add(supportMember);
    await _context.SaveChangesAsync();
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
        SenderName = x.Sender.Name
      })
      .OrderBy(x => x.CreatedAt)
      .ToListAsync();
      return messages;
  }
  public async Task<List<MessageGetResDto>?> ListMessage_v2(long userId)
  {
    var messages = await _context.MessageConversationMembers
      .Where(x => x.UserId == userId && x.DeletedAt == null)
      .SelectMany(y => y.Conversation.MessageMessages)
      .Select(z => new MessageGetResDto
      {
        SenderId = z.SenderId,
        SenderName = z.Sender.Name,
        Message = z.Message,
        MessageId = z.Id,
        CreatedAt = z.CreatedAt
      })
      .OrderByDescending(z => z.CreatedAt)
      .Take(100)
      .OrderBy(z => z.CreatedAt)
      .ToListAsync();
    return messages;
  }
  public async Task<List<MessageGetResDto>?> ListMessageBySupport_v2(long conversationId)
  {
    var messages = await _context.MessageConversationMembers
      .Where(x => x.ConversationId == conversationId && x.DeletedAt == null && x.User.UserType.Code != "Support" && x.User.UserType.Code != "Admin")
      .SelectMany(y => y.Conversation.MessageMessages)
      .Select(z => new MessageGetResDto
      {
        SenderId = z.SenderId,
        SenderName = z.Sender.Name,
        Message = z.Message,
        MessageId = z.Id,
        CreatedAt = z.CreatedAt
      })
      .OrderByDescending(z => z.CreatedAt)
      .Take(100)
      .OrderBy(z => z.CreatedAt)
      .ToListAsync();
    return messages;
  }
  public async Task<List<ContactGetResDto>?> ListContact_v2(long userId, string conversationState)
  {
    if(conversationState == "active")
    {
      var acontact = await _context.MessageConversationMembers
        .Where(x => x.UserId == userId && x.DeletedAt == null && x.Conversation.State == conversationState && x.Conversation.CreatedAt > DateTime.Now.AddMinutes(-30))
        .Select(x => new ContactGetResDto
        {
          ConversationId = x.Conversation.Id,
          NameContact = x.Conversation.Name ?? "Unknonw",
          PhoneContact = "0122",
          PreviewMessage = x.Conversation.MessageMessages
            .OrderByDescending(y => y.CreatedAt)
            .Select(y =>  new PreviewMessageResDto
            {
              SenderId = y.SenderId,
              SenderName = y.Sender.Name,
              LastMessage = y.Message,
              LastMessageId = y.Id,
              TimeLastMessage = y.CreatedAt,
              IsSeen = true
            }).FirstOrDefault()
        }).ToListAsync();
        return acontact;
    }
    var contact = await _context.MessageConversations
      .Where(x => x.State == conversationState && x.DeletedAt == null && x.CreatedAt > DateTime.Now.AddMinutes(-30))
      .Select(x => new ContactGetResDto
      {
        ConversationId = x.Id,
        NameContact = x.Name ?? "Unknonw",
        PhoneContact = "0122",
        PreviewMessage = x.MessageMessages
          .OrderByDescending(y => y.CreatedAt)
          .Select(y =>  new PreviewMessageResDto
          {
            SenderId = y.SenderId,
            SenderName = y.Sender.Name,
            LastMessage = y.Message,
            LastMessageId = y.Id,
            TimeLastMessage = y.CreatedAt,
            IsSeen = true
          }).FirstOrDefault()
      }).ToListAsync();
    return contact;
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
      .OrderByDescending(x => x.Conversation.MessageMessages
        .OrderByDescending(m => m.CreatedAt)
        .Select(m => (DateTime?)m.CreatedAt) // Ép kiểu sang Nullable để tránh lỗi nếu không có tin nhắn
        .FirstOrDefault() ?? x.CreatedAt)
      .Select(x => new ContactGetResDto
      {
        ConversationId = x.ConversationId,
        NameContact = _context.MessageConversationMembers
          .Where(y => y.UserId != userId && y.DeletedAt == null && y.ConversationId == x.ConversationId)
          .Select(y => y.User.Name)
          .FirstOrDefault() ?? "",
        PhoneContact = _context.MessageConversationMembers
          .Where(y => y.UserId != userId && y.DeletedAt == null && y.ConversationId == x.ConversationId)
          .Select(y => y.User.Phone)
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
          .FirstOrDefault() ?? new PreviewMessageResDto
          {
            LastMessageId = 0,
            LastMessage = "Chưa có tin nhắn",
            SenderId = 44,
            SenderName = "Admin",
            TimeLastMessage = x.CreatedAt,
            IsSeen = true,
          },
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
    if(member == null)
      return;
    member.LastSeenMessage = messageId;
    await _context.SaveChangesAsync();
  }
  public async Task<StateConversationDTO> SaveConversationState(long conversationId, string role)
  {
    var conversation = await _context.MessageConversations
      .Where(x => x.Id == conversationId && x.DeletedAt == null)
      .FirstOrDefaultAsync();
    if(conversation == null)
      throw new Exception("Khong tim thay cuoc hoi thoai");
    if((role == "Normal" || role == "Elite") && conversation.State == "close")
    {
      conversation.State = "waiting";
    }
    if((role == "Admin" || role == "Support") && conversation.State != "active")
    {
      conversation.State = "active";
    }
    await _context.SaveChangesAsync();
    var stateConversation = new StateConversationDTO
    {
      State = conversation.State,
      SupporterId = 0,
    };
    Console.WriteLine($"Truoc khi re nhanh conversationsatte {conversation.State}");
    if(conversation.State.Trim() == "active")
    {
      var userId = await _context.MessageConversationMembers
        .OrderByDescending(x => x.CreatedAt)
        .Where(x => x.ConversationId == conversationId && x.DeletedAt == null && (x.User.UserType.Code == "Admin" || x.User.UserType.Code == "Support"))
        .Select(x => x.UserId)
        .FirstOrDefaultAsync();
      stateConversation.SupporterId = userId;
      Console.WriteLine($"Dong nay den tu save conversationstate ne if xay ra  {stateConversation.SupporterId}");
    }
    return stateConversation;
  }
  public async Task AddSupportToConversation(long conversationId, long userId)
  {
    var conversationMember = await _context.MessageConversationMembers
      .Where(x => x.ConversationId == conversationId && x.UserId == userId)
      .AnyAsync();
    if(conversationMember)
      return;
    var user = await _context.UserCustomers
      .Where(x => x.Id == userId && x.DeletedAt == null)
      .Include(x => x.UserType)
      .FirstOrDefaultAsync();
    if(user == null || (user.UserType.Code != "Admin" && user.UserType.Code != "Support"))
    {
      throw new Exception("Nguoi dung ko hop le");
    }
    var conversation = await _context.MessageConversations
      .Where(x => x.Id == conversationId && x.DeletedAt == null)
      .FirstOrDefaultAsync();
    if(conversation == null || conversation.State == "active")
    {
      throw new Exception("Tham gia doan chat that bai");
    }
    conversation.State = "active";
    var newConversationMemeber = new MessageConversationMember
    {
      UserId = userId,
      CreatedAt = DateTime.Now,
      ConversationId = conversationId,
    };
    _context.MessageConversationMembers.Add(newConversationMemeber);
    await _context.SaveChangesAsync();
  }
  public async Task MarkConversationDone(long conversationId)
  {
    var conversation = await _context.MessageConversations
      .Where(x => x.Id == conversationId && x.DeletedAt == null)
      .FirstOrDefaultAsync();
    if(conversation == null)
      return;
    conversation.State = "close";
    await _context.SaveChangesAsync();
  }
}