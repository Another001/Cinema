using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Interfaces;
public interface ICinemaService : IServiceScoped
{
  Task<CinemaGetResDto?> GetCinema(long id);
  Task<List<CinemaGetResDto>?> ListCinema(CinemaFilterDto dto);
  Task<CinemaCinema> CreateCinema(CinemaCreateReqDto dto);
  Task<CinemaCinema> UpdateCinema(long id, CinemaUpdateReqDto dto);
  //Room
  Task<CinemaRoom?> GetRoom(long id);
  Task<List<RoomGetResDto>?> ListRoom(RoomFilterDto dto);
  Task<CinemaRoom> CreateRoom(RoomCreateReqDto dto);
  Task<CinemaRoom> UpdateRoom(long id, RoomUpdateReqDto dto);
}