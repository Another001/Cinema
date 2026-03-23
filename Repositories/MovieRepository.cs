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
  public async Task<MovieMovie?> GetMovie(long id)
  {
    var movie = await
      _context.MovieMovies
      .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
    return movie;
  }
  public async Task<List<MovieGetResDto>> ListMovie(MovieFilterDto dto)
  {
    var query = ConvertFilterDTOToFilterEntity(dto);
    var movies =
      await(
        from movie in query
        join status in _context.MovieMovieStatuses
        on movie.MovieStatusId equals status.Id
        select new MovieGetResDto
        {
          Id = movie.Id,
          Name = movie.Name,
          Title = movie.Title,
          Duration = movie.Duration,
          MovieStatus = status.Code
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
      query = query.Where(x => x.Name == dto.Name);
    }
    if (dto.Duration.HasValue)
    {
      query = query.Where(x => x.Duration <= dto.Duration);
    }
    return query;
  }
}