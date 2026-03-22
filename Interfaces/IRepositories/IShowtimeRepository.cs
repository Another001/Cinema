using MyApi.Models;
using MyApi.DTOs;

namespace MyApi.Interfaces;
public interface IShowtimeRepository
{
  Task<List<ShowtimeGetResDto>> ListShowtime(ShowtimeFilterDto dto);
  Task<MovieShowtime> CreateShowtime(MovieShowtime dto);
  Task<MovieShowtime> UpdateShowtime(long id, ShowtimeUpdateReqDto dto);
}