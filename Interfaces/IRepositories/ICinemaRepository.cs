using MyApi.Models;
using MyApi.DTOs;

namespace MyApi.Interfaces;
public interface ICinemaRepository
{
  Task<CinemaCinema?> GetCinema(long id);
  Task<List<CinemaGetResDto>?> ListCinema(CinemaFilterDto query);
  Task<CinemaCinema> CreateCinema(CinemaCinema newCinema);
  Task<CinemaCinema?> UpdateCinema(long id,  CinemaUpdateReqDto dto);

  //Room
  Task<CinemaRoom?> GetRoom(long id);
  Task<List<RoomGetResDto>?> ListRoom(RoomFilterDto query);
  Task<CinemaRoom> CreateRoom(CinemaRoom newRoom);
  Task<CinemaRoom?> UpdateRoom(long id,  RoomUpdateReqDto dto);

  //Seat
  Task<SeatGetResDTO?> GetSeat(long id);
  Task<List<SeatGetResDTO>?> ListSeat(SeatFilterDTO dto);
  Task<CinemaSeat> CreateSeat(CinemaSeat newSeat);
  Task<CinemaSeat?> UpdateSeat(long id, SeatUpdateReqDTO dto);
}