using System;
using System.Collections.Generic;

namespace MyApi.Models;

public partial class CinemaSeatType
{
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public string? Color { get; set; }

    public virtual ICollection<BookingSeatPrice> BookingSeatPrices { get; set; } = new List<BookingSeatPrice>();

    public virtual ICollection<CinemaSeat> CinemaSeats { get; set; } = new List<CinemaSeat>();
}
