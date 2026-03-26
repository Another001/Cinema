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
  public async Task<List<ShowtimeGetResDto>> ListShowtime(ShowtimeFilterDto dto)
  {
    var query = ConvertFilterDTOToFilterEntity(dto);
    var showtimes =
      await(
        from showtime in query
        join room in _context.CinemaRooms
        on showtime.RoomId equals room.Id
        join movie in _context.MovieMovies
        on showtime.MovieId equals movie.Id
        join cinema in _context.CinemaCinemas
        on room.CinemaId equals cinema.Id
        select new ShowtimeGetResDto
        {
          Id = showtime.Id,
          MovieName = movie.Name,
          RoomName = room.Name,
          BeginAt = showtime.BeginAt,
          EndAt = showtime.EndAt,
          CinemaAddress = cinema.Address,
          SeatPrices = _context.BookingSeatPrices
            .Where(p => p.ShowtimeId == showtime.Id && p.DeletedAt == null)
            .Select(p => new SeatPriceGetResDto
            {
              SeatType = p.SeatType.Code, 
              Price = p.SeatPrice
            }).ToList()
        }
      ).ToListAsync();
    return showtimes;
  }
  public async Task<MovieShowtime> CreateShowtime(MovieShowtime newShowtime, List<BookingSeatPrice> newSeatPrice)
  {
    _context.MovieShowtimes.Add(newShowtime);
    foreach(var price in newSeatPrice)
    {
      price.Showtime = newShowtime;
    }
    _context.BookingSeatPrices.AddRange(newSeatPrice);
    await _context.SaveChangesAsync();
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
    if (dto.MovieId.HasValue)
    {
      query = query.Where(x => x.MovieId == dto.MovieId);
    }
    return query;
  }
}