using MyApi.Models;
using MyApi.DTOs;

namespace MyApi.Interfaces;
public interface IMovieRepository
{
  Task<MovieGetResDto?> GetMovie(long id);
  Task<List<MovieListResDto>> ListMovie(MovieFilterDto dto);
  Task<List<MovieListResDto>> ListMovieNow();
  Task<List<MovieListResDto>> ListMovieUpcoming();
  Task<MovieMovie> CreateMovie(MovieMovie dto);
  Task<MovieMovie?> UpdateMovie(long id, MovieUpdateDto dto);
}