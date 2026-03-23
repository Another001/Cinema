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
          CinemaAddress = cinema.Address
        }
      ).ToListAsync();
    return showtimes;
  }
  public async Task<MovieShowtime> CreateShowtime(MovieShowtime newShowtime)
  {
    _context.MovieShowtimes.Add(newShowtime);
    await _context.SaveChangesAsync();
    return newShowtime;
  }
  public async Task<MovieShowtime?> UpdateShowtime(long id, ShowtimeUpdateReqDto dto)
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
    _context.Entry(showtime).State = EntityState.Modified;
    await _context.SaveChangesAsync();
    return showtime;
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