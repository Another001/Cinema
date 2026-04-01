using System;
using System.Collections.Generic;

namespace MyApi.Models;

public partial class MovieMovie
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Describe { get; set; }

    public long Duration { get; set; }

    public bool Used { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid RowId { get; set; }

    public long MovieStatusId { get; set; }

    public DateTime ReleaseDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Genre { get; set; } = null!;

    public string Director { get; set; } = null!;

    public string Cast { get; set; } = null!;

    public string Figure { get; set; } = null!;

    public string? Language { get; set; }

    public string? Trailer { get; set; }

    public virtual ICollection<MovieShowtime> MovieShowtimes { get; set; } = new List<MovieShowtime>();

    public virtual MovieMovieStatus MovieStatus { get; set; } = null!;
}
