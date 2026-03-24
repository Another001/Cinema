using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace MyApi.DTOs;
public class SeatGetResDTO
{
  public long Id {set; get;}
  public string Name{set; get;} = string.Empty;
  public string SeatType{set; get;} = string.Empty;
  public string SeatStatus{get; set;} = string.Empty;
  public string Room{get; set;} = string.Empty;
  public string Cinema{get; set;} = string.Empty;
}

public class SeatCreateReqDTO
{
  public required long RoomId{set; get;}
  public required string Name{get; set;}
  public long SeatStatusId{set; get;} = 1;
  public long SeatTypeId{set; get;} = 1;
}

public class SeatUpdateReqDTO
{
  public string? Name{get; set;}
  public long? SeatStatusId{set; get;}
  public long? SeatTypeId{set; get;}
}

public class SeatFilterDTO
{
  public long? RoomId {set; get;}
}