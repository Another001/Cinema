using System;
using System.Collections.Generic;

namespace MyApi.Models;

public partial class BookingTicket
{
    public long Id { get; set; }

    public long ReservationId { get; set; }

    public long SeatId { get; set; }

    public long TicketStatusId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid RowId { get; set; }

    public virtual BookingReservation Reservation { get; set; } = null!;

    public virtual CinemaSeat Seat { get; set; } = null!;

    public virtual BookingTicketStatus TicketStatus { get; set; } = null!;
}
