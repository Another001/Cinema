using MyApi.Repositories;
using MyApi.DTOs;
using MyApi.Interfaces;
using MyApi.Models;
using System.Diagnostics;

namespace MyApi.Services;
public class CinemaService : ICinemaService
{
  private readonly ICinemaRepository _userRepo;

  public CinemaService(ICinemaRepository userRepo) => _userRepo = userRepo;

  //CINEMA
  public async Task<CinemaGetResDto?> GetCinema(long id)
    {
    var cinema = await _userRepo.GetCinema(id);
    if (cinema == null)
    {
      throw new Exception("Khong tim thay rap chieu phim");
    }
    return new CinemaGetResDto {
        City = cinema.City,
        Address = cinema.Address,
        CinemaStatus = cinema.CinemaStatus?.Code ?? "unknow",
    };
  }

  public async Task<List<CinemaGetResDto>?> ListCinema(CinemaFilterDto dto)
  {
    var cinemas = await _userRepo.ListCinema(dto);
    return cinemas;
  }

  public async Task<CinemaCinema> CreateCinema(CinemaCreateReqDto dto)
  {
    if (string.IsNullOrEmpty(dto.City))
    {
      throw new Exception("Ten thanh pho khong duoc de trong");
    }
    if (string.IsNullOrEmpty(dto.Address))
    {
      throw new Exception("Dia chi khong duoc de trong");
    }
    var newCinema = CinemaConvertDTOToEntity(dto);
    await _userRepo.CreateCinema(newCinema);
    return newCinema;
  }

  public async Task<CinemaCinema> UpdateCinema(long id, CinemaUpdateReqDto dto)
  {
    try
    {
      var cinema = await _userRepo.UpdateCinema(id, dto);
      if(cinema == null)
        throw new Exception("Khong tim thay rap phim");
      return cinema;
    }
    catch
    {
      throw; 
    }
  }

  //ROOM
  public async Task<CinemaRoom?> GetRoom(long id)
  {
    var room = await _userRepo.GetRoom(id);
    if (room == null)
    {
      throw new Exception("Khong tim thay phong chieu");
    }
    return room;
  }
  public async Task<List<RoomGetResDto>?> ListRoom(RoomFilterDto dto)
  {
    var rooms = await _userRepo.ListRoom(dto);
    return rooms;
  }
  public async Task<CinemaRoom> CreateRoom(RoomCreateReqDto dto)
  {
    var newRoom = RoomConvertDTOToEntity(dto);
    await _userRepo.CreateRoom(newRoom);
    return newRoom;
  }
  public async Task<CinemaRoom> UpdateRoom(long id, RoomUpdateReqDto dto)
  {
    try
    {
      var room = await _userRepo.UpdateRoom(id, dto);
      if(room == null)
      {
        throw new Exception("Khong tim thay phong chieu");
      }
      return room;
    }
    catch
    {
      throw; 
    }
  }
  //Helper
  private CinemaCinema CinemaConvertDTOToEntity(CinemaCreateReqDto dto)
  {
    var cinema = new CinemaCinema
    {
      City = dto.City,
      Address = dto.Address,
      CinemaStatusId = dto.CinemaStatusId,
      CreatedAt = DateTime.Now,
      UpdatedAt = DateTime.Now,
      RowId = Guid.NewGuid(),
    };
    return cinema;
  }
  private CinemaRoom RoomConvertDTOToEntity(RoomCreateReqDto dto)
  {
    var room = new CinemaRoom
    {
      CinemaId = dto.CinemaId,
      Name = dto.Name,
      RoomStatusId = dto.RoomStatusId,
      RoomTypeId = dto.RoomTypeId,
      CreatedAt = DateTime.Now,
      UpdatedAt = DateTime.Now,
      RowId = Guid.NewGuid(),
    };
    return room;
  }
}