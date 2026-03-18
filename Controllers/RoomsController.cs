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
        select new RoomGetResDto
        {
          Id = room.Id,
          Name = room.Name,
          RoomStatus = status.Code,
          RoomType = type.Code,
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
