using System;
using System.Collections.Generic;

namespace MyApi.Models;

public partial class MovieMovieStatusEnum
{
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public string? Color { get; set; }

    public virtual ICollection<MovieMovie> MovieMovies { get; set; } = new List<MovieMovie>();
}
