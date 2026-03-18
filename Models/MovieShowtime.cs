using System;
using System.Collections.Generic;

namespace MyApi.Models;

public partial class MovieShowtime
{
    public long Id { get; set; }

    public long MovieId { get; set; }

    public long RoomId { get; set; }

    public DateTime BeginAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid RowId { get; set; }

    public long ShowtimeStatusId { get; set; }

    public DateTime EndAt { get; set; }

    public virtual ICollection<BookingReservation> BookingReservations { get; set; } = new List<BookingReservation>();

    public virtual ICollection<BookingSeatPrice> BookingSeatPrices { get; set; } = new List<BookingSeatPrice>();

    public virtual ICollection<BookingTicket> BookingTickets { get; set; } = new List<BookingTicket>();

    public virtual MovieMovie Movie { get; set; } = null!;

    public virtual CinemaRoom Room { get; set; } = null!;

    public virtual MovieShowtimeStatus ShowtimeStatus { get; set; } = null!;
}
