using MyApi.Repositories;
using MyApi.DTOs;
using MyApi.Interfaces;
using MyApi.Models;

namespace MyApi.Services;

public class MockShowtimeService : IShowtimeService
{
  private readonly IShowtimeRepository  _useRepo;
  private readonly IMovieRepository _useRepoMovie;
  private readonly ICinemaRepository _useRepoCinema;
  public MockShowtimeService(IShowtimeRepository useRepo, IMovieRepository useRepoMovie, ICinemaRepository useRepoCinema)
  {
    _useRepo = useRepo;
    _useRepoCinema = useRepoCinema;
    _useRepoMovie = useRepoMovie;
  }
  public async Task<List<ShowtimeGetResDto>?> ListShowtime(ShowtimeFilterDto dto)
  {
    var showtimes = await _useRepo.ListShowtime(dto);
    return showtimes;
  }
  public async Task<ShowtimeGetResDto> CreateShowtime(ShowtimeCreateReqDto dto)
  {
    var isRoomExist = await _useRepoCinema.GetRoom(dto.RoomId);
    if (isRoomExist == null)
    {
      throw new Exception("Khong ton tai phong chieu");
    }
    var isMovieExist = await _useRepoMovie.GetMovie(dto.MovieId);
    if (isMovieExist == null)
    {
      throw new Exception("Khong ton tai phim");
    }
    var newShowtime = ConvertDTOToEntity(dto);
    await _useRepo.CreateShowtime(newShowtime);
    return new ShowtimeGetResDto
    {
      Id = newShowtime.Id,
      RoomName = newShowtime.RoomId.ToString(),
      MovieName = newShowtime.MovieId.ToString(),
      BeginAt = newShowtime.BeginAt,
      EndAt = newShowtime.EndAt,
      CinemaAddress = newShowtime.RoomId.ToString(),
    };
  }
  public async Task<MovieShowtime> UpdateShowtime(long id, ShowtimeUpdateReqDto dto)
  {
    try
    {
      var newShowtime = await _useRepo.UpdateShowtime(id, dto);
      if(newShowtime == null)
      {
        throw new Exception("Khong tim thay suat chieu");
      }
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