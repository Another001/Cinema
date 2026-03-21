using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Models; 
using MyApi.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace MyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MockMovieController : ControllerBase
{
  private readonly IMovieService _useService;
  public MockMovieController(IMovieService useService)
  {
    _useService = useService;
  }
  [HttpGet]
  public async Task<ActionResult<List<MovieGetResDto>>> ListMovie([FromQuery] MovieFilterDto dto)
  {
    var movies = await _useService.ListMovie(dto);
    return Ok(movies);
  }
  [HttpPost]
  public async Task<ActionResult<MovieMovie>> CreateMovie([FromBody] MovieCreateReqDto dto)
  {
    var newMovie = await _useService.CreateMovie(dto);
    return Ok(newMovie);
  }
  [HttpPut("{id}")]
  public async Task<ActionResult<MovieMovie>> UpdateMovie([FromRoute] long id, [FromBody] MovieUpdateDto dto)
  {
    try
    {
      var movie = await _useService.UpdateMovie(id, dto);
      return Ok(movie);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }
}