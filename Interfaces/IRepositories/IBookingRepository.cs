using MyApi.Models;
using MyApi.DTOs;

namespace MyApi.Interfaces;
public interface IBookingRepository
{
  public Task<List<ShowtimeSeatGetResDTO>> GetShowtimeSeats(long id);
  public Task<BookingReservationGetDTO> CreateReservation(BookingReservationCreateDTO dto);
  public Task<BookingReservationGetDTO?> GetReservation(long id);
  public Task<List<TicketGetResDTO>> ConfirmReservation(long id);
}