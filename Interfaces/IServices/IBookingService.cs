using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Interfaces;
public interface IBookingService
{
  public Task<List<ShowtimeSeatGetResDTO>> GetShowtimeSeats(long id);
  public Task<BookingReservationGetDTO> CreateReservation(BookingReservationCreateDTO dto);
  public Task<BookingReservationGetDTO> GetReservation(long id);
  public Task<List<TicketGetResDTO>> ConfirmReservation(long id);
}