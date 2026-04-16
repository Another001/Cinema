using System;
using System.Collections.Generic;

namespace MyApi.Models;

public partial class MovieComment
{
    public long CustomerId { get; set; }

    public long MovieId { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long Id { get; set; }

    public virtual UserCustomer Customer { get; set; } = null!;

    public virtual MovieMovie Movie { get; set; } = null!;
}
