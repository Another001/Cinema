using System;
using System.Collections.Generic;

namespace MyApi.Models;

public partial class MessageMessage
{
    public long Id { get; set; }

    public long ConversationId { get; set; }

    public long SenderId { get; set; }

    public string Message { get; set; } = null!;

    public long Type { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual MessageConversation Conversation { get; set; } = null!;

    public virtual ICollection<MessageConversationMember> MessageConversationMembers { get; set; } = new List<MessageConversationMember>();

    public virtual UserCustomer Sender { get; set; } = null!;
}
