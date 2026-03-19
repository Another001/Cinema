using MyApi.Models;

namespace MyApi.DTOs
{
  public class RoomGetResDto
  {
    public required long Id {set; get;}
    public required string Name {set; get;}
    public string? RoomType {set; get;}
    public string? RoomStatus {get; set;}
    public required string Address {get; set;}
  }
  public class RoomCreateReqDto
  {
    public required long CinemaId {get; set;}
    public required string Name {get; set;}
    public required long RoomStatusId {get; set;}
    public required long RoomTypeId {get; set;}
  }
  public class RoomFilterDto
  {
    public long? CinemaId {get; set;}
    public long? RoomStatusId {get; set;}
    public long? RoomTypeId {get; set;}
  }
  public class RoomUpdateReqDto
  {
    public string? Name {set; get;}
    public long? CinemaId {set; get;}
    public long? RoomTypeId {set; get;}
    public long? RoomStatusId{set; get;}
  }
}