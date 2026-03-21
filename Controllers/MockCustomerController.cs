using Microsoft.AspNetCore.Mvc;
using MyApi.Interfaces;
using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MockCustomerController : ControllerBase
{
    private readonly ICustomerService _userService;
    public MockCustomerController(ICustomerService userService) => _userService = userService;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var result = await _userService.Get(id);
        if (result == null) return NotFound("Người dùng không tồn tại!");
        
        return Ok(result);
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
        var newCustomer = await _userService.Create(dto);
        return Ok(newCustomer);
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