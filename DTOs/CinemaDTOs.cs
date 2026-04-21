namespace MyApi.DTOs
{
  public class CreateStatusEnumCinemaReq
  {
    public required long Id {set; get;}
    public required string Code {set; get;}
    public string Name {get; set;} = string.Empty;
    public string Color {get; set;} = string.Empty;
  }
  public class CinemaCreateReqDto
  {
    public required string City {set; get;}
    public required string Name {set; get;}
    public required string Address{set; get;}
    public required string Phone{set; get;}
    public long CinemaStatusId {get; set;} = 1;
  }
  public class CinemaGetResDto
  {
    public string City {set; get;} = string.Empty;
    public string Name {set; get;} = string.Empty;
    public string CinemaStatus {get; set;} = string.Empty;
  }
  public class CinemaFilterDto
  {
    public string? City {set; get;}
    public long? CinemaStatusId {set; get;}
  }
  public class CinemaUpdateReqDto
  {
    public string? City{set; get;}
    public string? Address{set; get;}
    public string? Name{set; get;}
    public long? CinemaStatusId{set; get;}
  }
  public class CinemaListResGroupByCity
  {
    public required string City{set; get;}
    public required List<CinemaListResGroupByCinema> Cinemas{set; get;}
  }
  public class CinemaListResGroupByCinema
  {
    public required long CinemaId{set; get;}
    public required string Address{set; get;}
    public required string Name{set; get;}
    public required List<CinemaListResGroupByRoom> Rooms{set; get;}
  }
  public class CinemaListResGroupByRoom
  {
    public required long RoomId{set; get;}
    public required string RoomName{set; get;}
    public required string RoomType{set; get;}
  }
}