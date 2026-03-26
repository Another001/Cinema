using MyApi.Repositories;
using MyApi.DTOs;
using MyApi.Interfaces;
using MyApi.Models;

namespace MyApi.Services;
public class BookingService : IBookingService
{
  private readonly IBookingRepository _useRepo;
  public BookingService(IBookingRepository useRepo)
  {
    _useRepo = useRepo;
  }
  public async Task<List<ShowtimeSeatGetResDTO>> GetShowtimeSeats(long id)
  {
    var seats = await _useRepo.GetShowtimeSeats(id);
    return seats;
  }
  public async Task<BookingReservationGetDTO> CreateReservation(BookingReservationCreateDTO dto)
  {
    var result = await _useRepo.CreateReservation(dto);
    return result;
  }
  public async Task<BookingReservationGetDTO> GetReservation(long id)
  {
    var result = await _useRepo.GetReservation(id);
    if(result == null)
      throw new Exception("ko co thong tin dat ghe");
    return result;
  }
  public async Task<List<TicketGetResDTO>> ConfirmReservation(long id)
  {
    return await _useRepo.ConfirmReservation(id);
  }
}