using Microsoft.EntityFrameworkCore;
using MyApi.Models;
using MyApi.Interfaces;
using MyApi.DTOs;

namespace MyApi.Repositories;

public class ShowtimeRepository : IShowtimeRepository
{
  private readonly TestContext _context;
  public ShowtimeRepository(TestContext context)
  {
    _context = context;
  }
  public async Task<ShowtimeGetResDto?> GetShowtime(long id)
  {
    var showtime = await _context.MovieShowtimes
      .Where(x => x.Id == id && x.DeletedAt == null)
      .Select(s => new ShowtimeGetResDto
      {
        Id = s.Id,
        MovieName = s.Movie.Name,
        RoomName = s.Room.Name,
        CinemaAddress = s.Room.Cinema.Address,
        BeginAt = s.BeginAt,
        EndAt = s.EndAt,
        CinemaName = s.Room.Cinema.Name,
        SeatPrices = s.BookingSeatPrices
          .Where(x => x.DeletedAt == null)
          .Select(x => new SeatPriceGetResDto
          {
            SeatType = x.SeatType.Code,
            Price = x.SeatPrice
          }).ToList(),
      }).FirstOrDefaultAsync();
    return showtime;
  }
  public async Task<List<CityGroupResDto>> ListShowtime(ShowtimeFilterDto dto)
  {
    var query = ConvertFilterDTOToFilterEntity(dto);
    var result = await query
      .GroupBy(s => s.Room.Cinema.City)
      .Select(cityGroup => new CityGroupResDto
      {
        CityName = cityGroup.Key, 
        Cinemas = cityGroup
          .GroupBy(s => new { s.Room.Cinema.City, s.Room.Cinema.Address, s.Room.Cinema.Name})
          .Select(cinemaGroup => new CinemaGroupResDto
          {
            CinemaCity = cinemaGroup.Key.City,
            CinemaAddress = cinemaGroup.Key.Address,
            CinemaName = cinemaGroup.Key.Name,
            Showtimes = cinemaGroup.Select(s => new ShowtimeDetailDto
            {
              Id = s.Id,
              MovieName = s.Movie.Name,
              RoomName = s.Room.Name,
              BeginAt = s.BeginAt,
              EndAt = s.EndAt
            })
            .OrderBy(s => s.BeginAt)
            .ToList()
          })
          .ToList()
      })
      .ToListAsync();

    return result;
  }
  public async Task<List<AdminCityGroupResDto>?> AdminListShowtime(ShowtimeFilterDto dto)
  {
    var query = ConvertFilterDTOToFilterEntity(dto);
    var result = await query
      .GroupBy(s => s.Room.Cinema.City)
      .Select(cityGroup => new AdminCityGroupResDto
      {
        CityName = cityGroup.Key,
        Cinemas = cityGroup
          .GroupBy(cityGroup => cityGroup.Room.Cinema.Address)
          .Select(cinemaGroup => new AdminCinemaGroupResDto
          {
            CinemaAdress = cinemaGroup.Key,
            Rooms = cinemaGroup
              .GroupBy(cityGroup => cityGroup.Room.Name)
              .Select(roomGroup => new AdminRoomGroupResDto
              {
                RoomName = roomGroup.Key,
                Showtimes = roomGroup
                  .Select(x => new ShowtimeDetailDto
                  {
                    Id = x.Id,
                    MovieName = x.Movie.Name,
                    RoomName = x.Room.Name,
                    BeginAt = x.BeginAt,
                    EndAt = x.EndAt
                  }).ToList()
              }).ToList()
          }).ToList()
      }).ToListAsync();

    return result;
  }
  public async Task<MovieShowtime> CreateShowtime(MovieShowtime newShowtime, List<BookingSeatPrice> newSeatPrice)
  {
    var onTime = await _context.MovieShowtimes
      .Where(x => x.RoomId == newShowtime.RoomId
        && ((newShowtime.BeginAt >= x.BeginAt && newShowtime.BeginAt <= x.EndAt)
        || (newShowtime.EndAt >= x.BeginAt && newShowtime.EndAt <= x.EndAt)
        || (newShowtime.BeginAt <= x.BeginAt && newShowtime.EndAt >= x.EndAt))
        && x.DeletedAt == null)
        .FirstOrDefaultAsync();
 //       Console.WriteLine($"Conflict: {onTime?.Id} - {onTime?.BeginAt} - {onTime?.EndAt}");
 //    Console.WriteLine($"New Begin: {newShowtime.BeginAt:yyyy-MM-dd HH:mm:ss}");
 //     Console.WriteLine($"New End: {newShowtime.EndAt:yyyy-MM-dd HH:mm:ss}");
  //    Console.WriteLine($"Begin: {onTime?.BeginAt:yyyy-MM-dd HH:mm:ss}");
  //    Console.WriteLine($"End: {onTime?.EndAt:yyyy-MM-dd HH:mm:ss}");
    if (onTime != null)
    {
      throw new Exception("Phong nay da co lich chieu");
    }
    try
    {
       _context.MovieShowtimes.Add(newShowtime);
      foreach(var price in newSeatPrice)
      {
        price.Showtime = newShowtime;
      }
      _context.BookingSeatPrices.AddRange(newSeatPrice);
      await _context.SaveChangesAsync();
    } catch (Exception ex) {
      Console.WriteLine($"Thong bao loiiiiiii {ex.Message}");
      Console.WriteLine($"Noi loiiiiiiiii {ex.StackTrace}");
      throw;
    }
    return newShowtime;
  }
  public async Task<ShowtimeGetResDto?> UpdateShowtime(long id, ShowtimeUpdateReqDto dto)
  {
    var showtime = await _context.MovieShowtimes.FindAsync(id);
    if(showtime == null)
    {
      return null;
    }
    showtime.RoomId = dto.RoomId ?? showtime.RoomId;
    showtime.MovieId = dto.MovieId ?? showtime.MovieId;
    showtime.ShowtimeStatusId = dto.ShowtimeStatusId ?? showtime.ShowtimeStatusId;
    showtime.BeginAt = dto.BeginAt ?? showtime.BeginAt;
    showtime.EndAt = dto.EndAt ?? showtime.EndAt;
    showtime.UpdatedAt = DateTime.Now;
    if(dto.SeatPrices != null)
    {
      var oldPrice = await _context.BookingSeatPrices.Where(x => x.ShowtimeId == showtime.Id && x.DeletedAt == null).ToListAsync();
      foreach(var price in oldPrice)
      {
        price.DeletedAt = DateTime.Now;
      }
      foreach(var price in dto.SeatPrices)
      {
      _context.BookingSeatPrices.Add(new BookingSeatPrice
        {
          ShowtimeId = id,
          SeatTypeId = price.SeatTypeId,
          SeatPrice = price.Price,
          SeatPriceStatusId = 1,
          CreatedAt = DateTime.Now,
          UpdatedAt = DateTime.Now,
          RowId = Guid.NewGuid()
        });
      }
    }
    await _context.SaveChangesAsync();
    var result = await GetShowtime(id);
    return result;
  }
  //Helper
  private IQueryable<MovieShowtime> ConvertFilterDTOToFilterEntity(ShowtimeFilterDto dto)
  {
    var query = _context.MovieShowtimes.AsQueryable();
    if (!string.IsNullOrEmpty(dto.City))
    {
      query = query.Where(x => x.Room.Cinema.City == dto.City);
    }
    if (dto.MovieId.HasValue)
    {
      query = query.Where(x => x.MovieId == dto.MovieId);
    }
    if(dto.BeginAt != null)
    {
      query = query.Where(x => x.BeginAt.Date == dto.BeginAt);
    }
    return query;
  }
}
