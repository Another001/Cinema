using System;
using System.Collections.Generic;

namespace MyApi.Models;

public partial class MessageConversation
{
    public long Id { get; set; }

    public long Type { get; set; }

    public string? Name { get; set; }

    public string? Image { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<MessageConversationMember> MessageConversationMembers { get; set; } = new List<MessageConversationMember>();

    public virtual ICollection<MessageMessage> MessageMessages { get; set; } = new List<MessageMessage>();
}
