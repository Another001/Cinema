using MyApi.Models;
using MyApi.DTOs;

namespace MyApi.Interfaces;
public interface IMovieRepository
{
  Task<List<MovieGetResDto>> ListMovie(MovieFilterDto dto);
  Task<MovieMovie> CreateMovie(MovieMovie dto);
  Task<MovieMovie> UpdateMovie(long id, MovieUpdateDto dto);
}