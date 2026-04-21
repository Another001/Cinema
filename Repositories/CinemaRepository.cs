using Microsoft.EntityFrameworkCore;
using MyApi.Models;
using MyApi.Interfaces;
using MyApi.DTOs;
using System.Collections.Immutable;

namespace MyApi.Repositories;
public class CinemaRepository : ICinemaRepository
{
  private readonly TestContext _context;
  public CinemaRepository(TestContext context) => _context = context;

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
        Name = cinema.Name,
        CinemaStatus = status.Code
      }
    ).ToListAsync();
    return cinemas;
  }

  public async Task<List<CinemaListResGroupByCity>?> AdminListCinema(string? city)
{
    var query = _context.CinemaCinemas.AsQueryable();
    if (!string.IsNullOrEmpty(city))
    {
      query = query.Where(x => x.City == city);
    }
   var result = await query
        .GroupBy(x => x.City)   // Group theo City ngay tại Database
        .Select(g => new CinemaListResGroupByCity
        {
            City = g.Key,
            Cinemas = g.Select(x => new CinemaListResGroupByCinema
            {
                CinemaId = x.Id,
                Name = x.Name,
                Address = x.Address,
                Rooms = x.CinemaRooms.Select(y => new CinemaListResGroupByRoom
                {
                    RoomId = y.Id,
                    RoomName = y.Name,
                    RoomType = y.RoomType.Code
                }).ToList()
            }).ToList()
        })
        .ToListAsync();

    return result;
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
    cinema.Name = dto.Name ?? cinema.Name;
    cinema.CinemaStatusId = dto.CinemaStatusId ?? cinema.CinemaStatusId;
    cinema.UpdatedAt = DateTime.Now;
    await _context.SaveChangesAsync();
    return cinema;
  }

  //ROOM
  public async Task<CinemaRoom?> GetRoom(long id)
  {
    return await _context.CinemaRooms
      .Include(u => u.Cinema)
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
    var roomExist = await _context.CinemaRooms
      .Where(x => x.CinemaId == newRoom.CinemaId && x.Name == newRoom.Name && x.DeletedAt == null)
      .AnyAsync();
    if(roomExist)
      throw new Exception("Ten phong da ton tai");
    _context.CinemaRooms.Add(newRoom);
    await _context.SaveChangesAsync();
    var seats = new List<CinemaSeat>();
    for(char row = 'A'; row <= 'J'; row++)
    {
      for(int number = 1; number <= 10; number++)
      {
        string seatName = $"{row}{number}";
        long seatTypeId = 1;
        if('D' <= row && row <= 'I')
          seatTypeId = 2;
        if(row == 'J')
          seatTypeId = 3;
        seats.Add(new CinemaSeat
        {
          SeatTypeId = seatTypeId,
          RoomId = newRoom.Id,
          Name = seatName,
          CreatedAt = DateTime.Now,
          UpdatedAt = DateTime.Now,
          SeatStatusId = 1,
          RowId = Guid.NewGuid()
        });
      }
    }
    _context.CinemaSeats.AddRange(seats);
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

  //Seat
  public async Task<SeatGetResDTO?> GetSeat(long id)
  {
    var result = await _context.CinemaSeats
      .Include(u => u.SeatStatus)
      .Include(u => u.SeatType)
      .Include(u => u.Room)
        .ThenInclude(c => c.Cinema)
      .FirstOrDefaultAsync(u => u.Id == id && u.DeletedAt == null);
    if(result == null)
      return null;
    return new SeatGetResDTO
    {
      Id = result.Id,
      Name = result.Name,
      SeatType = result.SeatType.Code,
      SeatStatus = result.SeatStatus.Code,
      Cinema = result.Room.Cinema.Name,
      Room = result.Room.Name
    };
  }
  public async Task<CinemaSeat> CreateSeat(CinemaSeat newSeat)
  {
    _context.CinemaSeats.Add(newSeat);
    await _context.SaveChangesAsync();
    return newSeat;
  }
  public async Task<CinemaSeat?> UpdateSeat(long id, SeatUpdateReqDTO dto)
  {
    var seat = await _context.CinemaSeats.FindAsync(id);
    if(seat == null)
      return null;
    seat.Name = dto.Name ?? seat.Name;
    seat.SeatTypeId = dto.SeatTypeId ?? seat.SeatTypeId;
    seat.SeatStatusId = dto.SeatStatusId ?? seat.SeatStatusId;
    seat.UpdatedAt = DateTime.Now;
    await _context.SaveChangesAsync();
    return seat;
  }
  public async Task<List<SeatGetResDTO>?> ListSeat(SeatFilterDTO dto)
  {
    var query = SeatConvertFIlterDTOToFilterEntity(dto);
    var seats = await (
      from seat in query
      join type in _context.CinemaSeatTypes
      on seat.SeatTypeId equals type.Id
      join status in _context.CinemaSeatStatuses
      on seat.SeatStatusId equals status.Id
      join room in _context.CinemaRooms
      on seat.RoomId equals room.Id
      join cinema in _context.CinemaCinemas
      on room.CinemaId equals cinema.Id
      where seat.DeletedAt == null
      select new SeatGetResDTO
      {
        Id = seat.Id,
        Name = seat.Name,
        SeatType = type.Code,
        SeatStatus = status.Code,
        Cinema = cinema.Name,
        Room = room.Name,
      }
    ).ToListAsync();
    return seats;
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

  private IQueryable<CinemaSeat> SeatConvertFIlterDTOToFilterEntity(SeatFilterDTO dto)
  {
    var query = _context.CinemaSeats.AsQueryable();
    if (dto.RoomId.HasValue)
    {
      query = query.Where(x => x.RoomId == dto.RoomId);
    }
    return query;
  }
}