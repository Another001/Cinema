using Microsoft.AspNetCore.Mvc;
using MyApi.Models; 
using MyApi.DTOs;
using MyApi.Interfaces;

namespace MyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MockShowtimeController : ControllerBase
{
  private readonly IShowtimeService _useService;
  public MockShowtimeController(IShowtimeService useService)
  {
    _useService = useService;
  }
  [HttpGet]
  public async Task<ActionResult<List<ShowtimeGetResDto>>> ListShowtime([FromQuery] ShowtimeFilterDto dto)
  {
    var showtimes = await _useService.ListShowtime(dto);
    return Ok(showtimes);
  }
  [HttpPost]
  public async Task<ActionResult<MovieShowtime>> CreateShowtime([FromBody] ShowtimeCreateReqDto dto)
  {
    var newShowtime = await _useService.CreateShowtime(dto);
    return Ok(newShowtime);
  }
  [HttpPut("{id}")]
  public async Task<ActionResult<MovieShowtime>> UpdateShowtime([FromRoute] long id, [FromBody] ShowtimeUpdateReqDto dto)
  {
    try
    {
      var showtime = await _useService.UpdateShowtime(id, dto);
      return Ok(showtime);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }
}