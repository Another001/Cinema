using MyApi.Repositories;
using MyApi.DTOs;
using MyApi.Interfaces;
using MyApi.Models;

namespace MyApi.Services;

public class MockShowtimeService : IShowtimeService
{
  private readonly IShowtimeRepository  _useRepo;
  public MockShowtimeService(IShowtimeRepository useRepo)
  {
    _useRepo = useRepo;
  }
  public async Task<List<ShowtimeGetResDto>?> ListShowtime(ShowtimeFilterDto dto)
  {
    var showtimes = await _useRepo.ListShowtime(dto);
    return showtimes;
  }
  public async Task<MovieShowtime> CreateShowtime(ShowtimeCreateReqDto dto)
  {
    var newShowtime = ConvertDTOToEntity(dto);
    await _useRepo.CreateShowtime(newShowtime);
    return newShowtime;
  }
  public async Task<MovieShowtime> UpdateShowtime(long id, ShowtimeUpdateReqDto dto)
  {
    try
    {
      var newShowtime = await _useRepo.UpdateShowtime(id, dto);
      return newShowtime;
    }
    catch
    {
      throw; 
    }
  }
  //Helper
  private MovieShowtime ConvertDTOToEntity(ShowtimeCreateReqDto dto)
  {
    var newShowtime = new MovieShowtime
    {
      MovieId = dto.MovieId,
      RoomId = dto.RoomId,
      BeginAt = dto.BeginAt,
      EndAt = dto.EndAt,
      ShowtimeStatusId = 1,
      CreatedAt = DateTime.Now,
      UpdatedAt = DateTime.Now,
      RowId = Guid.NewGuid()
    };
    return newShowtime;
  }
}