using System;
using System.Collections.Generic;

namespace MyApi.Models;

public partial class CinemaSeat
{
    public long Id { get; set; }

    public long RoomId { get; set; }

    public string Name { get; set; } = null!;

    public long SeatTypeId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid RowId { get; set; }

    public long SeatStatusId { get; set; }

    public virtual ICollection<BookingReservationSeat> BookingReservationSeats { get; set; } = new List<BookingReservationSeat>();

    public virtual ICollection<BookingTicket> BookingTickets { get; set; } = new List<BookingTicket>();

    public virtual CinemaRoom Room { get; set; } = null!;

    public virtual CinemaSeatStatus SeatStatus { get; set; } = null!;

    public virtual CinemaSeatType SeatType { get; set; } = null!;
}
