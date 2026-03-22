using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Interfaces;
public interface IMovieService : IServiceScoped
{
  Task<List<MovieGetResDto>?> ListMovie(MovieFilterDto dto);
  Task<MovieMovie> CreateMovie(MovieCreateReqDto dto);
  Task<MovieMovie> UpdateMovie(long id, MovieUpdateDto dto);
}