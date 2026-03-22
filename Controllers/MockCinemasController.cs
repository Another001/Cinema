using Microsoft.AspNetCore.Mvc;
using MyApi.Interfaces;
using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MockCinemasController : ControllerBase
{
    private readonly ICinemaService _userService;
    public MockCinemasController(ICinemaService userService) => _userService = userService;
    //CINEMA
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCinema(long id)
    {
        var result = await _userService.GetCinema(id);
        if (result == null) return NotFound("Rap phim không tồn tại!");
        
        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult> ListCinema([FromQuery] CinemaFilterDto dto)
    {
        var result = await _userService.ListCinema(dto);
        return Ok(result);
    }
    [HttpPost]
    public async Task<ActionResult<CinemaCinema>> CreateCinema([FromBody] CinemaCreateReqDto dto)
    {
        var newCinema = await _userService.CreateCinema(dto);
        return Ok(newCinema);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<CinemaCinema>> UpdateCinema(long id, [FromBody] CinemaUpdateReqDto dto)
    {
        try
        {
            var cinema = await _userService.UpdateCinema(id, dto);
            return Ok(cinema);
        }
        catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    //ROOM
    [HttpGet("room/{id}")]
    public async Task<IActionResult> GetRoom(long id)
    {
        var result = await _userService.GetRoom(id);
        if (result == null) return NotFound("Phong Chieu không tồn tại!");
        return Ok(result);
    }
    [HttpGet("room")]
    public async Task<IActionResult> ListRoom([FromQuery] RoomFilterDto dto)
    {
        var result = await _userService.ListRoom(dto);
        return Ok(result);
    }
    [HttpPost("room")]
    public async Task<ActionResult<CinemaRoom>> CreateRoom([FromBody] RoomCreateReqDto dto)
    {
        var newRoom = await _userService.CreateRoom(dto);
        return Ok(newRoom);
    }
    [HttpPut("room/{id}")]
    public async Task<ActionResult<CinemaRoom>> UpdateRoom(long id, [FromBody] RoomUpdateReqDto dto)
    {
        try
        {
            var room = await _userService.UpdateRoom(id, dto);
            return Ok(room);
        }
        catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}