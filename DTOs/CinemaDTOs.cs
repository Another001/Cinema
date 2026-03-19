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
    public required string Address {set; get;}
    public long CinemaStatusId {get; set;} = 1;
  }
  public class CinemaGetResDto
  {
    public string City {set; get;} = string.Empty;
    public string Address {set; get;} = string.Empty;
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
    public long? CinemaStatusId{set; get;}
  }
}