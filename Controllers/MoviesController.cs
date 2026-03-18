  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using MyApi.Models;
  using MyApi.DTOs;

  namespace MyApi.Controllers
  {
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
      private readonly TestContext _context;
      public MoviesController(TestContext context)
      {
        _context = context;
      }
      [HttpGet]
      public async Task<ActionResult<List<MovieGetResDto>>> List([FromQuery] MovieFilterDto filterDto)
      {
        var query  = ConvertFilterDTOToFilterEntity(filterDto);
        var finalQuery = 
          from movie in query
          join status in _context.MovieMovieStatuses
          on movie.MovieStatusId equals status.Id
          select new
          {
            Id = movie.Id,
            Name = movie.Name,
            Tile = movie.Title,
            Describe = movie.Describe,
            MovieStatus = status.Code
          };
        var movies = await finalQuery.ToListAsync();
        return Ok(movies);
      }
      [HttpPost]
      public async Task<IActionResult> createMovie ([FromBody] MovieCreateReqDto dto)
      {
        var newMovie = ConvertDTOToEntity(dto);
        _context.MovieMovies.Add(newMovie);
        await _context.SaveChangesAsync();
        return Ok(newMovie);
      }

      // HELPER
      private IQueryable<MovieMovie> ConvertFilterDTOToFilterEntity(MovieFilterDto filterDto)
      {
        var query  = _context.MovieMovies.AsQueryable();
        if(!string.IsNullOrEmpty(filterDto.Name))
        {
          query = query.Where(x => x.Name.Contains(filterDto.Name));
        }
        if (filterDto.Duration.HasValue)
        {
          query = query.Where(x => x.Duration >= filterDto.Duration);
        }
        return query;
      }
      private MovieMovie ConvertDTOToEntity (MovieCreateReqDto createReqDto)
      {
        var entity = new MovieMovie
        {
          Name = createReqDto.Name,
          Title = createReqDto.Title,
          Duration = createReqDto.Duration,
          Describe = createReqDto.Describe,
          MovieStatusId = createReqDto.MovieStatusId,
          Used = createReqDto.Used,
          CreatedAt = DateTime.Now,
          UpdatedAt = DateTime.Now,
          RowId = Guid.NewGuid(),
        };
        return entity;
      }
    }
  }