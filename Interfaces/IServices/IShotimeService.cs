using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Interfaces;
public interface IShowtimeService : IServiceScoped
{
  Task<List<ShowtimeGetResDto>?> ListShowtime(ShowtimeFilterDto dto);
  Task<ShowtimeGetResDto> CreateShowtime(ShowtimeCreateReqDto dto);
  Task<ShowtimeGetResDto> UpdateShowtime(long id, ShowtimeUpdateReqDto dto);
}