namespace MyApi.DTOs;

public class MessageGetResDto
{
  public long MessageId{set; get;}
  public string Message{set; get;} = string.Empty;
  public DateTime CreatedAt{set; get;}
  public long SenderId{set; get;}
  public string SenderName{set; get;} = string.Empty;
}

public class ContactGetResDto
{
  public long ConversationId{set; get;}
  public string NameContact{set; get;} = string.Empty;
  public string PhoneContact{set; get;} = string.Empty;
  public PreviewMessageResDto? PreviewMessage{set; get;}
}

public class ContactGetResDtoCombine
{
  public List<ContactGetResDto>? WaitingContatct{set; get;}
  public List<ContactGetResDto>? ActiveContact{set; get;}
}

public class PreviewMessageResDto
{
  public long LastMessageId{set; get;}
  public string LastMessage{set; get;} = string.Empty;
  public string SenderName{set; get;} = string.Empty;
  public long SenderId{set; get;}
  public DateTime TimeLastMessage{set; get;}
  public bool IsSeen{set; get;}
}

public class StateConversationDTO
{
  public long SupporterId{set; get;}
  public required string State{set; get;}
}