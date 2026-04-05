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
      .Where(x => x.Reservation.ShowtimeId == dto.ShowtimeId && x.DeletedAt == null && x.Reservation.ExpiredAt > now)
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
        Id = r.Id,
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
    // 1. Dùng UtcNow nếu DB lưu chuẩn UTC, hoặc giữ .Now nếu DB lưu giờ Local
    var now = DateTime.Now; 

    var reservation = await _context.BookingReservations
        .FirstOrDefaultAsync(r => r.Id == id && r.DeletedAt == null);

    if (reservation == null)
    {
        throw new Exception("Khong tim thay thong tin dat cho");
    }

    // 2. Kiểm tra xem chính reservation này đã hết hạn chưa
    if (reservation.ExpiredAt < now)
    {
        throw new Exception("Thoi gian giu cho da het han, vui long dat lai");
    }

    // 3. Lấy danh sách SeatId của đơn hàng này
    var reservationSeats = await _context.BookingReservationSeats
        .Where(x => x.ReservationId == id && x.DeletedAt == null)
        .Select(x => x.SeatId)
        .ToListAsync();

    // 4. Kiểm tra xem có ai khác đã thanh toán hoặc đang giữ chỗ "còn hạn" không
    // Tách riêng 2 điều kiện cho rõ ràng và nhanh hơn
    
    // Check xem có ai đang giữ chỗ (Reservation) mà chưa hết hạn không
    var hasOtherActiveReservation = await _context.BookingReservationSeats
        .AnyAsync(rs => 
            reservationSeats.Contains(rs.SeatId) 
            && rs.ReservationId != id 
            && rs.Reservation.ShowtimeId == reservation.ShowtimeId
            && rs.Reservation.ExpiredAt > now // Chỉ tính những thằng CÒN HẠN
            && rs.Reservation.DeletedAt == null // Đảm bảo không tính hàng đã xóa
            && rs.DeletedAt == null);

    // Check xem có ai đã xuất vé (Ticket) thật sự chưa
    var hasTicket = await _context.BookingTickets
        .AnyAsync(t => 
            reservationSeats.Contains(t.SeatId) 
            && t.ShowtimeId == reservation.ShowtimeId
            && t.DeletedAt == null); // Nếu bạn có dùng Soft Delete cho Ticket

    if (hasOtherActiveReservation || hasTicket)
    {
        throw new Exception("Ghe da duoc nguoi khac dat hoac dang trong qua trinh thanh toan");
    }

    // 5. Tiến hành tạo vé (giữ nguyên logic của bạn nhưng tối ưu hóa)
    var tickets = reservationSeats.Select(seatId => new BookingTicket
    {
        ShowtimeId = reservation.ShowtimeId,
        SeatId = seatId,
        CreatedAt = now,
        UpdatedAt = now,
        TicketStatusId = 1,
        RowId = Guid.NewGuid()
    }).ToList();

    _context.BookingTickets.AddRange(tickets);
    
    // Đánh dấu reservation này là đã hoàn tất (tùy logic của bạn, thường là xóa hoặc update status)
    // reservation.DeletedAt = now; 

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateException ex)
    {
        var innerException = ex.InnerException?.Message;
        throw new Exception($"Lỗi database: {innerException}");
    }

    // 6. Trả về kết quả (Dùng Include để tránh n+1 query)
    var ticketIds = tickets.Select(t => t.Id).ToList();
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
  public async Task<List<TicketGetResDTO>> ListTicketByUser(long id)
  {
    var Tickets = await _context.BookingTickets
      .Where(x => x.CustomerId == id && x.DeletedAt == null)
      .Select(x => new TicketGetResDTO
      {
        MovieName = x.Showtime.Movie.Name,
        Address = x.Showtime.Room.Cinema.Address,
        SeatName = x.Seat.Name,
        RoomName = x.Showtime.Room.Name,
        CreatedAt = x.CreatedAt
      }).ToListAsync();
    return Tickets;
  }
}