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
    await _useRepo.CreateShowtime(newShowtime.Showtime, newShowtime.SeatPrices);
    return new ShowtimeGetResDto
    {
      Id = newShowtime.Showtime.Id,
      RoomName = newShowtime.Showtime.RoomId.ToString(),
      MovieName = newShowtime.Showtime.MovieId.ToString(),
      BeginAt = newShowtime.Showtime.BeginAt,
      EndAt = newShowtime.Showtime.EndAt,
      CinemaAddress = newShowtime.Showtime.RoomId.ToString(),
    };
  }
  public async Task<ShowtimeGetResDto> UpdateShowtime(long id, ShowtimeUpdateReqDto dto)
  {
    var newShowtime = await _useRepo.UpdateShowtime(id, dto);
    if(newShowtime == null)
    {
      throw new Exception("Khong tim thay suat chieu");
    }
    return newShowtime;
  }
  //Helper
  private ShowtimeConversionResult ConvertDTOToEntity(ShowtimeCreateReqDto dto)
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
    List<BookingSeatPrice> newSeatPrice = new List<BookingSeatPrice>();
    foreach(var price in dto.SeatPrice)
    {
      var newPrice = new BookingSeatPrice
      {
        SeatTypeId = price.SeatTypeId,
        SeatPrice = price.Price,
        SeatPriceStatusId = 1,
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now,
        RowId = Guid.NewGuid(),
      };
      newSeatPrice.Add(newPrice);
    }
    var result = new ShowtimeConversionResult
    {
      Showtime = newShowtime,
      SeatPrices = newSeatPrice,
    };
    return result;
  }
}