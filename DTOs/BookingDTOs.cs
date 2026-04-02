namespace MyApi.DTOs;
public class ShowtimeSeatGetResDTO
{
  public long Id{set; get;}
  public string SeatName{set; get;} = string.Empty;
  public string SeatType{set; get;} = string.Empty;
  public bool? IsSeatEmpty{set; get;}
}

public class ShowtimeSeatCreateReqDTO
{
  public required long SeatId{set; get;}
}

public class BookingReservationCreateDTO
{
  public required long ShowtimeId{set; get;}
  public required List<ShowtimeSeatCreateReqDTO> Seats{set; get;}
  public required long CustomerId{set; get;}
}

public class BookingReservationGetDTO
{
  public long Id{set; get;}
  public string? MovieName{set; get;}
  public string? RoomName{set; get;}
  public List<ShowtimeSeatGetResDTO>? Seats{set; get;}
  public string? CustomerName{set; get;}
  public string? CustomerPhone{set; get;}
  public decimal? TotalPrice{set; get;}
  public long? ShowtimeId{set; get;}
}

public class TicketGetResDTO
{
  public string MovieName{set; get;} = string.Empty;
  public string Address{set; get;} = string.Empty;
  public string RoomName{set; get;} = string.Empty;
  public string SeatName{set; get;} = string.Empty;
  public DateTime CreatedAt{set; get;}
  public string? TicketSatus{set; get;}
}

public class TicketCreateReqDTO
{
  public required long ShowtimeId{set; get;}
  public required long SeatId{set; get;}
  public long TicketStatusId{set; get;} = 1;
}