using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Models; 
using MyApi.DTOs;
using Microsoft.IdentityModel.Tokens;

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
      var newCinema = ConvertDTOToEntity(dto);
      _context.CinemaCinemas.Add(newCinema);
      await _context.SaveChangesAsync();
      return Ok(newCinema);
    }
    [HttpGet]
    public async Task<ActionResult<List<CinemaGetResDto>>> getCinema([FromQuery] CinemaFilterDto dto)
    {
      var finalQuery = ConvertFilterDTOToEntity(dto);
      var cinemas = 
        await (from cinema in finalQuery
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
    [HttpPut("{id}")]
		public async Task<IActionResult> Update(long id, [FromBody] CinemaUpdateReqDto dto)
		{
			var cinema = await _context.CinemaCinemas.FindAsync(id);
			if (cinema == null)
			{
				return NotFound("Không tìm thấy rap để cập nhật!");
			}
			if (!string.IsNullOrEmpty(dto.City))
			{
				cinema.City = dto.City;
			}
			if (!string.IsNullOrEmpty(dto.Address))
			{
				cinema.Address = dto.Address;
			}
      if (dto.CinemaStatusId.HasValue)
      {
        cinema.CinemaStatusId = dto.CinemaStatusId;
      }
			cinema.UpdatedAt = DateTime.Now;
			_context.Entry(cinema).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return Ok(cinema);
		}
    //Helper
    private CinemaCinema ConvertDTOToEntity(CinemaCreateReqDto dto)
    {
      var entity = new CinemaCinema
      {
        City = dto.City,
        Address = dto.Address,
        CinemaStatusId = dto.CinemaStatusId,
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now,
        RowId = Guid.NewGuid(),
      };
      return entity;
    }
    private IQueryable<CinemaCinema> ConvertFilterDTOToEntity(CinemaFilterDto dto)
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
  }
}