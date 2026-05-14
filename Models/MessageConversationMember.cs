using System;
using System.Collections.Generic;

namespace MyApi.Models;

public partial class MessageConversationMember
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long ConversationId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual MessageConversation Conversation { get; set; } = null!;

    public virtual UserCustomer User { get; set; } = null!;
}
