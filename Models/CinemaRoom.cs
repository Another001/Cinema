using System;
using System.Collections.Generic;

namespace MyApi.Models;

public partial class CinemaRoom
{
    public long Id { get; set; }

    public long CinemaId { get; set; }

    public string Name { get; set; } = null!;

    public long RoomTypeId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid RowId { get; set; }

    public long RoomStatusId { get; set; }

    public virtual CinemaCinema Cinema { get; set; } = null!;

    public virtual ICollection<CinemaSeat> CinemaSeats { get; set; } = new List<CinemaSeat>();

    public virtual ICollection<MovieShowtime> MovieShowtimes { get; set; } = new List<MovieShowtime>();

    public virtual CinemaRoomStatus RoomStatus { get; set; } = null!;

    public virtual CinemaRoomType RoomType { get; set; } = null!;
}
