using MyApi.Models;

namespace MyApi.DTOs
{
  public class ShowtimeCreateReqDto
  {
    public required long MovieId {get; set;}
    public required long RoomId {get; set;}
    public required DateTime BeginAt{get; set;}
    public required DateTime EndAt {get; set;}
    public long ShowtimeStatusId {get; set;} = 1;
    public required List<SeatPriceCreateReqDto> SeatPrice{get; set;}
  }
  public class SeatPriceCreateReqDto
  {
    public required long SeatTypeId{set; get;}
    public required decimal Price {set; get;}
  }
  public class SeatPriceGetResDto
  {
    public string SeatType{set; get;} = string.Empty;
    public decimal Price{set; get;}
  }
  public class ShowtimeGetResDto
  {
    public required long Id {set; get;}
    public required string RoomName {get; set;}
    public required string MovieName {get; set;}
    public required DateTime BeginAt {get; set;}
    public required DateTime EndAt {get; set;}
    public required string CinemaAddress {get; set;}
    public required string CinemaName{set; get;}
    public List<SeatPriceGetResDto> SeatPrices{set; get;} = [];
  }
  public class ShowtimeListResDto
  {
    public required long Id {set; get;}
    public required string RoomName {get; set;}
    public required string MovieName {get; set;}
    public required DateTime BeginAt {get; set;}
    public required DateTime EndAt {get; set;}
    public required string CinemaAddress {get; set;}
    public required string City{set; get;}
  }
  public class CityGroupResDto
  {
    public string? movieName {set; get;}
    public string? CityName { get; set; }
    public List<CinemaGroupResDto>? Cinemas { get; set; }
  }
  public class AdminCityGroupResDto
  {
    public string? CityName{get; set;}
    public List<AdminCinemaGroupResDto>? Cinemas{set; get;}
  }
  public class AdminCinemaGroupResDto
  {
    public string? CinemaAdress{set; get;}
    public List<AdminRoomGroupResDto>? Rooms{set; get;}
  }
  public class AdminRoomGroupResDto
  {
    public string? RoomName{set; get;}
    public List<ShowtimeDetailDto>? Showtimes{get; set;}
  }

  public class CinemaGroupResDto
  {
      public string? CinemaName { get; set; }
      public string? CinemaAddress { get; set; }
      public string? CinemaCity{set; get;}
      public List<ShowtimeDetailDto>? Showtimes { get; set; }
  }

  public class ShowtimeDetailDto
  {
      public long Id { get; set; }
      public string? MovieName { get; set; }
      public string? RoomName { get; set; }
      public DateTime BeginAt { get; set; }
      public DateTime EndAt { get; set; }
  }
  public class ShowtimeFilterDto
  {
    public long? MovieId {get; set;}
    public string? City {set; get;}
    public DateTime? BeginAt{set; get;}
  }
  public class ShowtimeUpdateReqDto
  {
    public long? RoomId {set; get;}
    public long? MovieId {set; get;}
    public long? ShowtimeStatusId {set; get;}
    public DateTime? BeginAt {set; get;}
    public DateTime? EndAt {set; get;}
    public List<SeatPriceCreateReqDto>? SeatPrices{set; get;}
  }
  public class ShowtimeConversionResult
  {
    public MovieShowtime Showtime { get; set; } = null!;
    public List<BookingSeatPrice> SeatPrices { get; set; } = new();
  }
}