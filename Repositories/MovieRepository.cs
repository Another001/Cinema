using Microsoft.EntityFrameworkCore;
using MyApi.Models;
using MyApi.Interfaces;
using MyApi.DTOs;

namespace MyApi.Repositories;

public class MovieRepository : IMovieRepository
{
  private readonly TestContext _context;
  public MovieRepository(TestContext context)
  {
    _context = context;
  }
  public async Task<MovieGetResDto?> GetMovie(long id)
  {
    var movie = await
      _context.MovieMovies
      .Where(x => x.Id == id && x.DeletedAt == null)
      .Select(x => new MovieGetResDto
      {
        Id = x.Id,
        Name = x.Name,
        Describe = x.Describe ?? "",
        Title = x.Title,
        Duration = x.Duration,
        Genre = x.Genre,
        Cast = x.Cast,
        Director = x.Director,
        ReleaseDate = x.ReleaseDate,
        Figure = x.Figure,
        Language = x.Language ?? "",
        Trailer = x.Trailer?? "",
      })
      .FirstOrDefaultAsync();
    return movie;
  }
  public async Task<List<MovieListResDto>> ListMovieNow()
  {
    var now = DateTime.Now;
    var movies = await
      _context.MovieMovies
      .Where(x => x.ReleaseDate <= now && x.EndDate >= now)
      .OrderByDescending(x => x.ReleaseDate)
      .Select(x => new MovieListResDto
      {
        Id = x.Id,
        Name = x.Name,
        Duration = x.Duration,
        Genre = x.Genre,
        ReleaseDate = x.ReleaseDate,
        Figure = x.Figure
      })
      .ToListAsync();
    return movies;
  }
    public async Task<List<MovieListResDto>> ListMovieUpcoming()
  {
    var now = DateTime.Now;
    var movies = await
      _context.MovieMovies
      .Where(x => x.ReleaseDate >= now)
      .OrderBy(x => x.ReleaseDate)
      .Select(x => new MovieListResDto
      {
        Id = x.Id,
        Name = x.Name,
        Duration = x.Duration,
        Genre = x.Genre,
        ReleaseDate = x.ReleaseDate,
        Figure = x.Figure
      })
      .ToListAsync();
    return movies;
  }
  public async Task<List<MovieListResDto>> ListMovie(MovieFilterDto dto)
  {
    var query = ConvertFilterDTOToFilterEntity(dto);
    var movies =
      await(
        from movie in query
        join status in _context.MovieMovieStatuses
        on movie.MovieStatusId equals status.Id
        select new MovieListResDto
        {
          Id = movie.Id,
          Name = movie.Name,
          Duration = movie.Duration,
          Genre = movie.Genre,
          ReleaseDate = movie.ReleaseDate,
          Figure = movie.Figure
        }
      ).ToListAsync();
    return movies;
  }
  public async Task<MovieMovie> CreateMovie(MovieMovie newMovie)
  {
    _context.MovieMovies.Add(newMovie);
    await _context.SaveChangesAsync();
    return newMovie;
  }
  public async Task<MovieMovie?> UpdateMovie(long id, MovieUpdateDto dto)
  {
    var movie = await _context.MovieMovies.FindAsync(id);
    if(movie == null)
    {
      return null;
    }
    if (!string.IsNullOrEmpty(dto.Name))
    {
      movie.Name = dto.Name;
    }
    if (!string.IsNullOrEmpty(dto.Describe))
    {
      movie.Describe = dto.Describe;
    }
    if (!string.IsNullOrEmpty(dto.Title))
    {
      movie.Title = dto.Title;
    }
    if (dto.Duration.HasValue)
    {
      movie.Duration = dto.Duration.Value;
    }
    if (dto.MovieStatusId.HasValue)
    {
      movie.MovieStatusId = dto.MovieStatusId.Value;
    }
    movie.ReleaseDate = dto.ReleaseDate ?? movie.ReleaseDate;
    movie.Cast = dto.Cast ?? movie.Cast;
    movie.EndDate = dto.EndDate ?? movie.EndDate;
    movie.Director = dto.Director ?? movie.Director;
    movie.Genre = dto.Genre ?? movie.Genre;
    movie.Language = dto.Language ?? movie.Language;
    movie.Trailer = dto.Trailer ?? movie.Trailer;
    movie.UpdatedAt = DateTime.Now;
    _context.Entry(movie).State = EntityState.Modified;
    await _context.SaveChangesAsync();
    return movie;
  }
  //Helper
  private IQueryable<MovieMovie> ConvertFilterDTOToFilterEntity(MovieFilterDto dto)
  {
    var query = _context.MovieMovies.AsQueryable();
    if (!string.IsNullOrEmpty(dto.Name))
    {
      query = query.Where(x => x.Name.Contains(dto.Name));
    }
    if (dto.Duration.HasValue)
    {
      query = query.Where(x => x.Duration <= dto.Duration);
    }
    return query;
  }
}