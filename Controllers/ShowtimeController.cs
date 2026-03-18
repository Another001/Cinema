using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Models;
using MyApi.DTOs;

namespace MyApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ShowtimeController : ControllerBase
  {
    private readonly TestContext _context;
    public ShowtimeController (TestContext context)
    {
      _context = context;
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ShowtimeCreateReqDto createReqDto)
    {
      var newShowtime = ConvertDTOToEntity(createReqDto);
      _context.MovieShowtimes.Add(newShowtime);
      await _context.SaveChangesAsync();
      return Ok(newShowtime);
    }

    [HttpGet]
    public async Task<ActionResult<List<ShowtimeGetResDto>>> Get([FromQuery] ShowtimeFilterDto showtimeFilterDto)
    {
      var query = ConvertFilterDTOToFilterEntity(showtimeFilterDto);
      var finalQuery = 
        from showtime in query
        join movie in _context.MovieMovies
        on showtime.MovieId equals movie.Id
        join room in _context.CinemaRooms
        on showtime.RoomId equals room.Id
        join cinema in _context.CinemaCinemas
        on room.CinemaId equals cinema.Id
        select new ShowtimeGetResDto
        {
          Id = showtime.Id,
          MovieName = movie.Name,
          CinemaAddress = cinema.Address,
          RoomName = room.Name,
          BeginAt = showtime.BeginAt,
          EndAt = showtime.EndAt,
        };
      var showtimes = await finalQuery.ToListAsync();
      return showtimes;
    }
    //Helper
    private MovieShowtime ConvertDTOToEntity(ShowtimeCreateReqDto createReqDto)
    {
      var entity = new MovieShowtime
      {
        MovieId = createReqDto.MovieId,
        RoomId = createReqDto.RoomId,
        BeginAt = createReqDto.BeginAt,
        EndAt = createReqDto.EndAt,
        ShowtimeStatusId = createReqDto.ShowtimeStatusId,
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now,
        RowId = Guid.NewGuid()
      };
      return entity;
    }
    private IQueryable<MovieShowtime> ConvertFilterDTOToFilterEntity(ShowtimeFilterDto showtimeFilterDto)
    {
      var query = _context.MovieShowtimes.AsQueryable();
      if (showtimeFilterDto.MovieId.HasValue)
      {
        query = query.Where(x => x.MovieId == showtimeFilterDto.MovieId);
      }

      return query;
    }
  }
}