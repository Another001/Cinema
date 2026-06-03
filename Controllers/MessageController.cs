using Microsoft.AspNetCore.Mvc;
using MyApi.Interfaces;
using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
	private readonly IMessageService _messageService;
  public MessageController(IMessageService messageService)
  {
    _messageService = messageService;
  }
  [HttpGet("ChatList/{conversationId}")]
  public async Task<ActionResult<List<MessageGetResDto>?>> ListMessage([FromRoute] long conversationId)
  {
    try
    {
      var messages = await _messageService.ListMessage(conversationId);
      return Ok(messages);
    }
    catch(Exception ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }
  [HttpGet("ListMessage-v2/{userId}")]
  public async Task<ActionResult<List<MessageGetResDto>?>> ListMessage_v2([FromRoute] long userId)
  {
    try
    {
      var messages = await _messageService.ListMessage_v2(userId);
      return Ok(messages);
    }
    catch(Exception ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }
  [HttpGet("CreateConversation/{userId}")]
  public async Task<ActionResult<long>> CreateConversationByUser([FromRoute] long userId)
  {
    try
    {
      var conversationId = await _messageService.CreateConversationByUser(userId);
      return Ok(conversationId);
    }
    catch(Exception ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }
  [HttpGet("GetConversation/{userId}")]
  public async Task<ActionResult<long>> GetOrCreateConversation([FromRoute] long userId)
  {
    try
    {
      var conversationId = await _messageService.GetOrCreateConversation(userId);
      return Ok(conversationId);
    }
    catch(Exception ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }
  [HttpGet("ListContact/{userId}")]
  public async Task<ActionResult<ContactGetResDto>> ListContact([FromRoute] long userId)
  {
    try
    {
      var contacts = await _messageService.ListContact(userId);
      return Ok(contacts);
    }
    catch(Exception ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }
  [HttpGet("ListContact-v2")]
  public async Task<ActionResult<List<ContactGetResDto>?>> ListContact_v2([FromQuery] long userId, string conversationState)
  {
    try
    {
      var contacts = await _messageService.ListContact_v2(userId, conversationState);
      return Ok(contacts);
    }
    catch(Exception ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }
  [HttpGet("ListMessageBySupport/{conversationId}")]
  public async Task<ActionResult<List<MessageGetResDto>?>> ListContactBySupport([FromRoute] long conversationId)
  {
    try
    {
      var messages = await _messageService.ListMessageBySupport_v2(conversationId);
      return Ok(messages);
    }
    catch(Exception ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }
  [HttpGet("AddSupportToConversation")]
 public async Task<ActionResult> AddSupportToConversation([FromQuery] long conversationId, long userId)
  {
    try
    {
      await _messageService.AddSupportToConversation(conversationId, userId);
      return Ok();
    }
    catch(Exception ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }
  
}