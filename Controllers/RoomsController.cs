using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Models;
using MyApi.DTOs;

namespace MyApi.Controllers
{
  [ApiController]
	[Route("api/[controller]")]
  public class RoomsController : ControllerBase
  {
    private readonly TestContext _context;
    public RoomsController(TestContext context)
    {
      _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<List<RoomGetResDto>>> Get([FromQuery] RoomFilterDto roomFilterDto)
    {
      var query = ConvertFilterDTOToFIlterEntity(roomFilterDto);
      var finalQuery = 
        from room in query
        join status in _context.CinemaRoomStatuses
        on room.RoomStatusId equals status.Id
        join type in _context.CinemaRoomTypes
        on room.RoomTypeId equals type.Id
        join cinema in _context.CinemaCinemas
        on room.CinemaId equals cinema.Id
        select new RoomGetResDto
        {
          Id = room.Id,
          Name = room.Name,
          RoomStatus = status.Code,
          RoomType = type.Code,
          Address = cinema.Address,
        };
      var rooms = await finalQuery.ToListAsync();
      return rooms;
    }
    [HttpPost]
    public async Task<IActionResult> Create(RoomCreateReqDto roomCreateReqDto)
    {
      var newRoom = ConvertDTOToEntity(roomCreateReqDto);
      _context.CinemaRooms.Add(newRoom);
      await _context.SaveChangesAsync();
      return Ok(newRoom);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] RoomUpdateReqDto dto)
    {
      var room = await _context.CinemaRooms.FindAsync(id);
      if(room == null)
        return NotFound("Khong tim thay phong chieu");
      room.Name = dto.Name ?? room.Name;
      room.CinemaId = dto.CinemaId ?? room.CinemaId;
      room.RoomStatusId = dto.RoomStatusId ?? room.RoomStatusId;
      room.RoomTypeId = dto.RoomTypeId ?? room.RoomTypeId;
      room.UpdatedAt = DateTime.Now;
      _context.Entry(room).State = EntityState.Modified;
      await _context.SaveChangesAsync();
      return Ok(room);
    }

    //Helper
    private IQueryable<CinemaRoom> ConvertFilterDTOToFIlterEntity(RoomFilterDto roomFilterDto)
    {
      var query = _context.CinemaRooms.AsQueryable();
      if (roomFilterDto.CinemaId.HasValue)
      {
        query = query.Where(x => x.CinemaId == roomFilterDto.CinemaId);
      }
      if (roomFilterDto.RoomStatusId.HasValue)
      {
        query = query.Where(x => x.RoomStatusId == roomFilterDto.RoomStatusId);
      }
      if (roomFilterDto.RoomTypeId.HasValue)
      {
        query = query.Where(x => x.RoomTypeId == roomFilterDto.RoomTypeId);
      }
      return query;
    }

    private CinemaRoom ConvertDTOToEntity(RoomCreateReqDto roomCreateReqDto)
    {
      var entity = new CinemaRoom
      {
        CinemaId = roomCreateReqDto.CinemaId,
        Name = roomCreateReqDto.Name,
        RoomTypeId = roomCreateReqDto.RoomTypeId,
        RoomStatusId = roomCreateReqDto.RoomStatusId,
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now,
        RowId = Guid.NewGuid()
      };
      return entity;
    }
  }
}
