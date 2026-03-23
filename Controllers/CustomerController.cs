using Microsoft.AspNetCore.Mvc;
using MyApi.Interfaces;
using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
	private readonly ICustomerService _userService;
	public CustomerController(ICustomerService userService) => _userService = userService;

	[HttpGet("{id}")]
	public async Task<IActionResult> Get(long id)
	{
		try
		{
			var result = await _userService.Get(id);
			return Ok(result);

		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}
	[HttpGet]
	public async Task<IActionResult> List([FromQuery] CustomerFilterDto dto)
	{
		var result = await _userService.List(dto);
		return Ok(result);
	}
	[HttpPost]
	public async Task<ActionResult<UserCustomer>> Create([FromBody] CustomerCreateReqDto dto)
	{
		try
		{
			var newCustomer = await _userService.Create(dto);
			return Ok(newCustomer);
		}
		catch(Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
		
	}
	[HttpPut("{id}")]
	public async Task<ActionResult<UserCustomer>> Update(long id, [FromBody] CustomerUpdateReqDto dto)
	{
		try
		{
			var customer = await _userService.Update(id, dto);
			return Ok(customer);
		}
		catch(Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}
}