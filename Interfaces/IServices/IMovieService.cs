using MyApi.DTOs;
using MyApi.Interfaces;
using MyApi.Models;

public interface IMovieService : IServiceScoped
{
  Task<List<MovieGetResDto>?> ListMovie(MovieFilterDto dto);
  Task<MovieMovie> CreateMovie(MovieCreateReqDto dto);
  Task<MovieMovie> UpdateMovie(long id, MovieUpdateDto dto);
}