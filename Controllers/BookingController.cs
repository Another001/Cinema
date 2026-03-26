using Microsoft.AspNetCore.Mvc;
using MyApi.Models; 
using MyApi.DTOs;
using MyApi.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MyApi.Controllers;
[Controller]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
  private readonly IBookingService _useService;
  public BookingController(IBookingService useService)
  {
    _useService = useService;
  }
  [HttpGet("ShowtimeSeats/{id}")]
  public async Task<ActionResult<List<ShowtimeSeatGetResDTO>>> GetShowtimeSeats([FromRoute] long id)
  {
    var seats = await _useService.GetShowtimeSeats(id);
    return Ok(seats);
  }
  [HttpPost("Reservation")]
  public async Task<ActionResult<BookingReservationGetDTO>> CreateReservation([FromBody] BookingReservationCreateDTO dto)
  {
    try
    {
      var result = await _useService.CreateReservation(dto);
      return Ok(result);
    }
    catch(Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
  }
  [HttpGet("Reservation/{id}")]
  public async Task<ActionResult<BookingReservationGetDTO>> GetReservation([FromRoute] long id)
  {
    try
    {
      var result = await _useService.GetReservation(id);
      return Ok(result);
    }
    catch(Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
  }
  [HttpGet("Confirm/{id}")]
  public async Task<ActionResult<TicketGetResDTO>> ConfirmReservation(long id)
  {
    try
    {
      var result = await _useService.ConfirmReservation(id);
      return Ok(result);
    }
    catch(Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
  }
}