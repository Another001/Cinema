using System;
using System.Collections.Generic;

namespace MyApi.Models;

public partial class BookingReservation
{
    public long Id { get; set; }

    public long CustomerId { get; set; }

    public long ShowtimeId { get; set; }

    public long ReservationStatusId { get; set; }

    public DateTime ExpiredAt { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid RowId { get; set; }

    public virtual ICollection<BookingReservationSeat> BookingReservationSeats { get; set; } = new List<BookingReservationSeat>();

    public virtual ICollection<BookingTicket> BookingTickets { get; set; } = new List<BookingTicket>();

    public virtual UserCustomer Customer { get; set; } = null!;

    public virtual BookingReservationStatus ReservationStatus { get; set; } = null!;

    public virtual MovieShowtime Showtime { get; set; } = null!;
}
