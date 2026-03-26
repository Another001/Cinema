using Microsoft.EntityFrameworkCore;
using MyApi.Models;
using MyApi.Interfaces;
using MyApi.DTOs;
using System.Text.Json;

namespace MyApi.Repositories;
public class BookingRepository : IBookingRepository
{
  private readonly TestContext _context;
  public BookingRepository(TestContext context)
  {
    _context = context;
  }
  public async Task<List<ShowtimeSeatGetResDTO>> GetShowtimeSeats(long id)
  {
    var now = DateTime.Now;
    var seats = await _context.MovieShowtimes
      .Where(x => x.Id == id && x.DeletedAt == null)
      .SelectMany(x => x.Room.CinemaSeats
        .Where(s => s.DeletedAt == null)
        .Select(s => new ShowtimeSeatGetResDTO
        {
          Id = s.Id,
          SeatName = s.Name,
          SeatType = s.SeatType.Code,
          IsSeatEmpty = !_context.BookingTickets
            .Any(t => t.ShowtimeId == id && t.SeatId == s.Id && t.DeletedAt == null)
          && !_context.BookingReservations
            .Any(r => r.ShowtimeId == id && r.DeletedAt == null && r.ExpiredAt > now
              && r.BookingReservationSeats.Any(rt => rt.SeatId == s.Id && rt.DeletedAt == null))
        })
      ).ToListAsync();
    return seats;
  }
  public async Task<BookingReservationGetDTO> CreateReservation(BookingReservationCreateDTO dto)
  {
    var now = DateTime.Now;
    //Validate
    var seats = new List<long>();
    foreach(var seat in dto.Seats)
    {
      seats.Add(seat.SeatId);
    }
    bool isSeatNotEmpty = await _context.BookingReservationSeats.AsNoTracking()
      .Where(x => x.Reservation.ShowtimeId == dto.ShowtimeId && x.DeletedAt == null)
      .AnyAsync(x => seats.Contains(x.SeatId));
    if (isSeatNotEmpty)
    {
      throw new Exception("Da co ghe dat roi");
    }
    decimal totalPrice = await (
      from seat in _context.CinemaSeats.AsNoTracking()
      join seatType in _context.CinemaSeatTypes.AsNoTracking()
      on seat.SeatTypeId equals seatType.Id
      join seatPrice in _context.BookingSeatPrices.AsNoTracking()
      on seatType.Id equals seatPrice.SeatTypeId
      where seat.DeletedAt == null
        && seats.Contains(seat.Id)
        && seatPrice.DeletedAt == null
        && seatPrice.ShowtimeId == dto.ShowtimeId
      select seatPrice.SeatPrice
    ).SumAsync();
    var reservation = new BookingReservation
    {
      CustomerId = dto.CustomerId,
      ShowtimeId = dto.ShowtimeId,
      ReservationStatusId = 1,
      ExpiredAt = now.AddMinutes(2),
      RowId = Guid.NewGuid(),
      CreatedAt = DateTime.Now,
      UpdatedAt = DateTime.Now,
      TotalPrice = totalPrice,
    };
    _context.BookingReservations.Add(reservation);
    var newBookingSeat = new List<BookingReservationSeat>();
    foreach(var seat in seats)
    {
      newBookingSeat.Add(new BookingReservationSeat
      {
        SeatId = seat,
        CreatedAt = now,
        UpdatedAt = now,
        Reservation = reservation,
      });
    }
    _context.BookingReservationSeats.AddRange(newBookingSeat);
    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateException ex)
    {
        // Đây là dòng quan trọng nhất để biết lỗi tại SQL hay tại Code
        var innerException = ex.InnerException?.Message;
        throw new Exception($"Lỗi database thật sự là: {innerException}");
    }
    var result = await GetReservation(reservation.Id);
    return result;
  }
  public async Task<BookingReservationGetDTO?> GetReservation(long id)
  {
    var result = await _context.BookingReservations
    .Where(r => r.Id == id)
    .Select(r => new BookingReservationGetDTO
    {
        MovieName = r.Showtime.Movie.Name,
        RoomName = r.Showtime.Room.Name,
        CustomerName = r.Customer.Name,
        CustomerPhone = r.Customer.Phone,
        TotalPrice = r.TotalPrice,
        ShowtimeId = r.ShowtimeId,
        Seats = r.BookingReservationSeats.Select(rs => new ShowtimeSeatGetResDTO
        {
            Id = rs.Seat.Id,
            SeatName = rs.Seat.Name,
            SeatType = rs.Seat.SeatType.Code,
        }).ToList()
    }).FirstOrDefaultAsync();
    return result;
  }
  public async Task<List<TicketGetResDTO>> ConfirmReservation(long id)
  {
    var now = DateTime.Now;
    var reservation = await _context.BookingReservations.FindAsync(id);
    if(reservation == null)
    {
      throw new Exception("Khong tim thay thong tin dat cho");
    }
    var reservationSeats = await _context.BookingReservationSeats
      .Where(x => x.DeletedAt == null && x.ReservationId == id)
      .Select(x => x.SeatId)
      .ToListAsync();
{/*    var isBooked = await (
      from ticket in _context.BookingTickets
      where ticket.DeletedAt == null
        && ticket.ShowtimeId == reservation.ShowtimeId
        && reservationSeats.Contains(ticket.SeatId)
      select ticket
    )
    var isBookedd = await(
      from seat in _context.BookingReservationSeats
      join reser in _context.BookingReservations
      on seat.Reservation.ShowtimeId equals reser.ShowtimeId
      where reser.ShowtimeId == reservation.ShowtimeId
        && reser.DeletedAt == null
        && reser.ExpiredAt > now
        && reservationSeats.Contains(seat.SeatId)
      select seat
    )
    */}
    var isBooked = await _context.CinemaSeats
    .Where(s => reservationSeats.Contains(s.Id))
    .AnyAsync(s => 
      _context.BookingReservationSeats.Any(rs => rs.SeatId == s.Id && rs.Reservation.ShowtimeId == reservation.ShowtimeId && rs.Reservation.ExpiredAt > now) ||
      _context.BookingTickets.Any(t => t.SeatId == s.Id && t.ShowtimeId == reservation.ShowtimeId)
    );
    if (isBooked)
    {
      throw new Exception("Ghe da duoc dat, khong the dat vao");
    }
    var dtickets = await (
      from bookingseat in _context.BookingReservationSeats
//      join bookingseat in _context.BookingReservationSeats
//      on bookingreservation.Id equals bookingseat.ReservationId
      where bookingseat.ReservationId == reservation.Id
      select new TicketCreateReqDTO
      {
        ShowtimeId = reservation.ShowtimeId,
        SeatId = bookingseat.SeatId,
      }).ToListAsync();
Console.WriteLine("Dữ liệu tickets: " + JsonSerializer.Serialize(dtickets, new JsonSerializerOptions { WriteIndented = true }));
    var tickets = new List<BookingTicket>();
    foreach(var dticket in dtickets)
    {
      tickets.Add(new BookingTicket
      {
        ShowtimeId = dticket.ShowtimeId,
        SeatId = dticket.SeatId,
        CreatedAt = now,
        UpdatedAt = now,
        TicketStatusId = 1,
        RowId = Guid.NewGuid()
      });
    }
    _context.BookingTickets.AddRange(tickets);
    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateException ex)
    {
        // Đây là dòng quan trọng nhất để biết lỗi tại SQL hay tại Code
        var innerException = ex.InnerException?.Message;
        throw new Exception($"Lỗi database thật sự là: {innerException}");
    }
    var ticketIds = new List<long>();
    foreach(var ticket in tickets)
    {
      ticketIds.Add(ticket.Id);
    }
    var result = await _context.BookingTickets
      .Where(x => ticketIds.Contains(x.Id))
      .Select(x => new TicketGetResDTO
      {
        Address = x.Showtime.Room.Cinema.Address,
        MovieName = x.Showtime.Movie.Name,
        RoomName = x.Showtime.Room.Name,
        SeatName = x.Seat.Name,
        CreatedAt = x.CreatedAt,
        TicketSatus = x.TicketStatus.Code,
      }).ToListAsync();
    return result;
  }
}