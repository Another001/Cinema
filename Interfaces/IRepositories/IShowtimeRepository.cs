using MyApi.Models;
using MyApi.DTOs;

namespace MyApi.Interfaces;
public interface IShowtimeRepository
{
  Task<ShowtimeGetResDto?> GetShowtime(long id);
  Task<List<CityGroupResDto>> ListShowtime(ShowtimeFilterDto dto);
  Task<MovieShowtime> CreateShowtime(MovieShowtime newShowtime, List<BookingSeatPrice> newSeatPrice);
  Task<ShowtimeGetResDto?> UpdateShowtime(long id, ShowtimeUpdateReqDto dto);
}