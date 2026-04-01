using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Interfaces;
public interface IMovieService : IServiceScoped
{
  Task<MovieGetResDto?> GetMovie(long id);
  Task<List<MovieListResDto>> ListMovie(MovieFilterDto dto);
  Task<MovieMovie> CreateMovie(MovieCreateReqDto dto);
  Task<MovieMovie> UpdateMovie(long id, MovieUpdateDto dto);
  Task<List<MovieListResDto>> ListMovieNow();
  Task<List<MovieListResDto>> ListMovieUpcoming();
}