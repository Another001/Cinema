using MyApi.Models;
using MyApi.DTOs;

namespace MyApi.Interfaces;
public interface IShowtimeRepository
{
  Task<List<ShowtimeGetResDto>> ListShowtime(ShowtimeFilterDto dto);
  Task<MovieShowtime> CreateShowtime(MovieShowtime newShowtime, List<BookingSeatPrice> newSeatPrice);
  Task<ShowtimeGetResDto?> UpdateShowtime(long id, ShowtimeUpdateReqDto dto);
}