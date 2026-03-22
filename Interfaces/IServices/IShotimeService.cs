using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Interfaces;
public interface IShowtimeService : IServiceScoped
{
  Task<List<ShowtimeGetResDto>?> ListShowtime(ShowtimeFilterDto dto);
  Task<MovieShowtime> CreateShowtime(ShowtimeCreateReqDto dto);
  Task<MovieShowtime> UpdateShowtime(long id, ShowtimeUpdateReqDto dto);
}