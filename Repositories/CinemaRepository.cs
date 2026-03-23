using Microsoft.EntityFrameworkCore;
using MyApi.Models;
using MyApi.Interfaces;
using MyApi.DTOs;

namespace MyApi.Repositories;
public class CinemaRepository : ICinemaRepository
{
  private readonly TestContext _context;
  public CinemaRepository(TestContext context) => _context = context;

  //CINEMA
  public async Task<CinemaCinema?> GetCinema(long id)
  {
    return await _context.CinemaCinemas
      .Include(u => u.CinemaStatus)
      .FirstOrDefaultAsync(u => u.Id == id && u.DeletedAt == null);
  }
 
  public async Task<List<CinemaGetResDto>?> ListCinema(CinemaFilterDto dto)
  {
    var query = CinemaConvertFilterDtoToFilterEntity(dto);
    var cinemas = await (
      from cinema in query
      join status in _context.CinemaCinemaStatuses
      on cinema.CinemaStatusId equals status.Id
      select new CinemaGetResDto
      {
        City = cinema.City,
        Address = cinema.Address,
        CinemaStatus = status.Code
      }
    ).ToListAsync();
    return cinemas;
  }
  
  public async Task<CinemaCinema> CreateCinema(CinemaCinema newCinema)
  {
    _context.CinemaCinemas.Add(newCinema);
    await _context.SaveChangesAsync();
    return newCinema;
  }
  
  public async Task<CinemaCinema?> UpdateCinema(long id, CinemaUpdateReqDto dto)
  {
    var cinema = _context.CinemaCinemas.Find(id);
    if(cinema == null)
    {
      return null;
    };
    cinema.City = dto.City ?? cinema.City;
    cinema.Address = dto.Address ?? cinema.Address;
    cinema.CinemaStatusId = dto.CinemaStatusId ?? cinema.CinemaStatusId;
    cinema.UpdatedAt = DateTime.Now;
    await _context.SaveChangesAsync();
    return cinema;
  }

  //ROOM
  public async Task<CinemaRoom?> GetRoom(long id)
  {
    return await _context.CinemaRooms
      .Include(u => u.RoomStatus)
      .FirstOrDefaultAsync(u => u.Id == id && u.DeletedAt == null);
  }

  public async Task<List<RoomGetResDto>?> ListRoom(RoomFilterDto dto)
  {
    var query = RoomConvertFilterDtoToFilterEntity(dto);
    var rooms = await (
      from room in query
      join status in _context.CinemaRoomStatuses
      on room.RoomStatusId equals status.Id
      join cinema in _context.CinemaCinemas
      on room.CinemaId equals cinema.Id
      join type in _context.CinemaRoomTypes
      on room.RoomTypeId equals type.Id
      select new RoomGetResDto
      {
        Id = room.Id,
        Name = room.Name,
        RoomType = type.Code,
        RoomStatus = status.Code,
        Address = cinema.Address
      }
    ).ToListAsync();
    return rooms;
  }

  public async Task<CinemaRoom> CreateRoom(CinemaRoom newRoom)
  {
    _context.CinemaRooms.Add(newRoom);
    await _context.SaveChangesAsync();
    return newRoom;
  }
  
  public async Task<CinemaRoom?> UpdateRoom(long id, RoomUpdateReqDto dto)
  {
    var room = _context.CinemaRooms.Find(id);
    if(room == null)
    {
      return null;
    };
    room.Name = dto.Name ?? room.Name;
    room.CinemaId = dto.CinemaId ?? room.CinemaId;
    room.RoomStatusId = dto.RoomStatusId ?? room.RoomStatusId;
    room.RoomTypeId = dto.RoomTypeId ?? room.RoomTypeId;
    room.UpdatedAt = DateTime.Now;
    await _context.SaveChangesAsync();
    return room;
  }
  //Helper
  private IQueryable<CinemaCinema> CinemaConvertFilterDtoToFilterEntity(CinemaFilterDto dto)
  {
    var query = _context.CinemaCinemas.AsQueryable();
    if (!string.IsNullOrEmpty(dto.City))
    {
      query = query.Where(x => x.City == dto.City);
    }
    if (dto.CinemaStatusId.HasValue)
    {
      query = query.Where(x => x.CinemaStatusId == dto.CinemaStatusId);
    }
    return query;
  }

  private IQueryable<CinemaRoom> RoomConvertFilterDtoToFilterEntity(RoomFilterDto dto)
  {
    var query = _context.CinemaRooms.AsQueryable();
    if (dto.CinemaId.HasValue)
    {
      query = query.Where(x => x.CinemaId == dto.CinemaId);
    }
    if (dto.RoomStatusId.HasValue)
    {
      query = query.Where(x => x.RoomStatusId == dto.RoomStatusId);
    }
    if (dto.RoomTypeId.HasValue)
    {
      query = query.Where(x => x.RoomTypeId == dto.RoomTypeId);
    }    
    return query;
  }
}