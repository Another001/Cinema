using Microsoft.AspNetCore.Mvc;
using MyApi.Interfaces;
using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CinemasController : ControllerBase
{
	private readonly ICinemaService _userService;
	public CinemasController(ICinemaService userService) => _userService = userService;
	//CINEMA
	[HttpGet("{id}")]
	public async Task<IActionResult> GetCinema(long id)
	{
		try{
			var result = await _userService.GetCinema(id);
			return Ok(result);
		}
		catch(Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
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
		try
		{
			var newCinema = await _userService.CreateCinema(dto);
			return Ok(newCinema);
		}
		catch(Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
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
	[HttpGet("Room/{id}")]
	public async Task<IActionResult> GetRoom(long id)
	{
		try
		{
			var result = await _userService.GetRoom(id);
			return Ok(result);
		}
		catch(Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}
	[HttpGet("Room")]
	public async Task<IActionResult> ListRoom([FromQuery] RoomFilterDto dto)
	{
		var result = await _userService.ListRoom(dto);
		return Ok(result);
	}
	[HttpPost("Room")]
	public async Task<ActionResult<CinemaRoom>> CreateRoom([FromBody] RoomCreateReqDto dto)
	{
		try
		{
			var newRoom = await _userService.CreateRoom(dto);
			return Ok(newRoom);	
		}
		catch(Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}
	[HttpPut("Room/{id}")]
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

	//Seat
	[HttpGet("Room/Seat/{id}")]
	public async Task<ActionResult<SeatGetResDTO>> GetSeat(long id)
	{
		try
		{
			var seat = await _userService.GetSeat(id);
			return Ok(seat);
		}
		catch(Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}
	[HttpPost("Room/Seat")]
	public async Task<ActionResult<CinemaSeat>> CreateSeat([FromBody] SeatCreateReqDTO dto)
	{
		try
		{
			var seat = await _userService.CreateSeat(dto);
			return Ok(seat);
		}
		catch(Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}
	[HttpGet("Room/Seat")]
	public async Task<ActionResult<List<SeatGetResDTO>>> ListSeat([FromQuery] SeatFilterDTO dto)
	{
		try
		{
			var seat = await _userService.ListSeat(dto);
			return Ok(seat);
		}
		catch(Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}
	[HttpPut("Room/Seat/{id}")]
	public async Task<ActionResult<CinemaSeat>> UpdateSeat(long id, [FromBody] SeatUpdateReqDTO dto)
	{
		try
		{
			var seat = await _userService.UpdateSeat(id, dto);
			return Ok(seat);
		}
		catch(Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}
}