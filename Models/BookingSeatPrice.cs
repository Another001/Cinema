using System;
using System.Collections.Generic;

namespace MyApi.Models;

public partial class BookingSeatPrice
{
    public long Id { get; set; }

    public long ShowtimeId { get; set; }

    public long SeatTypeId { get; set; }

    public decimal SeatPrice { get; set; }

    public long SeatPriceStatusId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid RowId { get; set; }

    public virtual BookingSeatPriceStatus SeatPriceStatus { get; set; } = null!;

    public virtual CinemaSeatType SeatType { get; set; } = null!;

    public virtual MovieShowtime Showtime { get; set; } = null!;
}
