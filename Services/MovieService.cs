
using MyApi.DTOs;
using MyApi.Interfaces;
using MyApi.Models;

namespace MyApi.Services;

public class MockMovieService : IMovieService
{
  private readonly IMovieRepository  _useRepo;
  public MockMovieService(IMovieRepository useRepo)
  {
    _useRepo = useRepo;
  }
  public async Task<MovieGetResDto?> GetMovie(long id)
  {
    var movie = await _useRepo.GetMovie(id);
    return movie;
  }
  public async Task<List<MovieListResDto>> ListMovie(MovieFilterDto dto)
  {
    var movies = await _useRepo.ListMovie(dto);
    return movies;
  }
  public async Task<List<MovieListResDto>> ListMovieNow()
  {
    var movies = await _useRepo.ListMovieNow();
    return movies;
  }
  public async Task<List<MovieListResDto>> ListMovieUpcoming()
  {
    var movies = await _useRepo.ListMovieUpcoming();
    return movies;
  }
  public async Task<MovieMovie> CreateMovie(MovieCreateReqDto dto)
  {
    if (string.IsNullOrEmpty(dto.Name))
    {
      throw new Exception("Ten khong duoc de trong");
    }
    var newMovie = ConvertDTOToEntity(dto);
    await _useRepo.CreateMovie(newMovie);
    return newMovie;
  }
  public async Task<MovieMovie> UpdateMovie(long id, MovieUpdateDto dto)
  {
    try
    {
      var newMovie = await _useRepo.UpdateMovie(id, dto);
      if(newMovie == null)
      {
        throw new Exception("Khong tim thay phim");
      }
      return newMovie;
    }
    catch
    {
      throw; 
    }
  }
  //Helper
  private MovieMovie ConvertDTOToEntity(MovieCreateReqDto dto)
  {
    var newMovie = new MovieMovie
    {
      Name = dto.Name,
      Title = dto.Title,
      Describe = dto.Describe,
      MovieStatusId = 1,
      ReleaseDate = dto.ReleaseDate,
      EndDate = dto.EndDate,
      Genre = dto.Genre,
      Director = dto.Director,
      Cast = dto.Cast,
      Used = true,
      Figure = dto.Figure,
      Trailer = dto.Trailer,
      Language = dto.Language,
      Duration = dto.Duration,
      CreatedAt = DateTime.Now,
      UpdatedAt = DateTime.Now,
      RowId = Guid.NewGuid(),
    };
    return newMovie;
  }
}