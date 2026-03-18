using System;
using System.Collections.Generic;

namespace MyApi.Models;

public partial class CinemaCinema
{
    public long Id { get; set; }

    public string City { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid RowId { get; set; }

    public long? CinemaStatusId { get; set; }

    public virtual ICollection<CinemaRoom> CinemaRooms { get; set; } = new List<CinemaRoom>();

    public virtual CinemaCinemaStatus? CinemaStatus { get; set; }
}
