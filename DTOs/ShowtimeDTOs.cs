using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace MyApi.DTOs
{
  public class ShowtimeCreateReqDto
  {
    public required long MovieId {get; set;}
    public required long RoomId {get; set;}
    public required DateTime BeginAt{get; set;}
    public required DateTime EndAt {get; set;}
    public long ShowtimeStatusId {get; set;} = 1;
  }
  public class ShowtimeGetResDto
  {
    public required long Id {set; get;}
    public required string RoomName {get; set;}
    public required string MovieName {get; set;}
    public required DateTime BeginAt {get; set;}
    public required DateTime EndAt {get; set;}
    public required string CinemaAddress {get; set;}
  }
  public class ShowtimeFilterDto
  {
    public long? MovieId {get; set;}
  }
  public class ShowtimeUpdateReqDto
  {
    public long? RoomId {set; get;}
    public long? MovieId {set; get;}
    public long? ShowtimeStatusId {set; get;}
    public DateTime? BeginAt {set; get;}
    public DateTime? EndAt {set; get;}
  }
}