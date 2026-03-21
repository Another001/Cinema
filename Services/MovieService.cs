using MyApi.Repositories;
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
  public async Task<List<MovieGetResDto>?> ListMovie(MovieFilterDto dto)
  {
    var movies = await _useRepo.ListMovie(dto);
    return movies;
  }
  public async Task<MovieMovie> CreateMovie(MovieCreateReqDto dto)
  {
    var newMovie = ConvertDTOToEntity(dto);
    await _useRepo.CreateMovie(newMovie);
    return newMovie;
  }
  public async Task<MovieMovie> UpdateMovie(long id, MovieUpdateDto dto)
  {
    try
    {
      var newMovie = await _useRepo.UpdateMovie(id, dto);
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
      Used = true,
      Duration = dto.Duration,
      CreatedAt = DateTime.Now,
      UpdatedAt = DateTime.Now,
      RowId = Guid.NewGuid(),
    };
    return newMovie;
  }
}