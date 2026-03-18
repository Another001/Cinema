using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Models; // Đây là chỗ bạn gọi cái "Thư mục ảo" chứa class UserCustomer
using MyApi.DTOs;


namespace MyApi.Controllers
{
  [ApiController]
	[Route("api/[controller]")]
  public class CinemasController : ControllerBase
  {
    private readonly TestContext _context;
    public CinemasController(TestContext context)
    {
      _context = context;
    }
    [HttpPost("createStatusEnum")]
    public async Task<IActionResult> createStatusEnumCinema ([FromBody] CreateStatusEnumCinemaReq dto)
    {
      var newEnum = new CinemaCinemaStatus
      {
        Id = dto.Id,
        Code = dto.Code,
        Name = dto.Name,
        Color = dto.Color,
      };
      _context.CinemaCinemaStatuses.Add(newEnum);
      await _context.SaveChangesAsync();
      return Ok(newEnum);
    }
    [HttpPost]
    public async Task<IActionResult> createCinema ([FromBody] CinemaCreateReqDto dto)
    {
      var newCinema = new CinemaCinema
      {
        City = dto.City,
        Address = dto.Address,
        CinemaStatusId = dto.CinemaStatusId,
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now,
        RowId = Guid.NewGuid(),
      };
      _context.CinemaCinemas.Add(newCinema);
      await _context.SaveChangesAsync();
      return Ok(newCinema);
    }
    [HttpGet]
    public async Task<ActionResult<List<CinemaGetResDto>>> getCinema()
    {
      var cinemas = 
        await (from cinema in _context.CinemaCinemas
          join status in _context.CinemaCinemaStatuses
          on cinema.CinemaStatusId equals status.Id
          select new
          {
            City = cinema.City,
            Address = cinema.Address,
            Status = status.Name,
          }
        ).ToListAsync();
      return Ok(cinemas);
    }
  }
}