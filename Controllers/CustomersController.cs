using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Models;
using MyApi.DTOs;

namespace MyApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CustomersController : ControllerBase
	{
		private readonly TestContext _context;
		
		public CustomersController(TestContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<List<CustomerGetResDto>>> List([FromQuery] CustomerFilterDto dto)
		{
			var filterDto = ConvertFilterDTOToFilterEntity(dto);
			var finalQuery = 
				from user in filterDto
				join status in _context.UserCustomerStatuses
				on user.UserStatusId equals status.Id
				select new CustomerGetResDto
				{
						Name = user.Name,
						Phone = user.Phone,
						Email = user.Email,
						Status = status.Name,
				};
			var customers = await finalQuery.ToListAsync();
			return Ok(customers);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CustomerGetResDto>> Get(long id)
		{
			var customer = await (
				from user in _context.UserCustomers
				join status in _context.UserCustomerStatuses
				on user.UserStatusId equals status.Id
				where user.Id == id
				select new CustomerGetResDto
				{
					Name = user.Name,
					Phone = user.Phone,
					Email = user.Email,
					Status = status.Name
				}
			).FirstOrDefaultAsync();

			if (customer == null) return NotFound("Không tìm thấy khách hàng này!");

			return Ok(customer);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CustomerCreateReqDto dto)
		{
			var newUser = ConvertDTOToEntity(dto);
			_context.UserCustomers.Add(newUser);
			await _context.SaveChangesAsync();
			return Ok(newUser);
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(long id, [FromBody] CustomerUpdateReqDto dto)
		{
			var customer = await _context.UserCustomers.FindAsync(id);
			if (customer == null)
			{
				return NotFound("Không tìm thấy khách hàng để cập nhật!");
			}
			if (!string.IsNullOrEmpty(dto.Name))
			{
				customer.Name = dto.Name;
			}
			if (!string.IsNullOrEmpty(dto.Email))
			{
				customer.Email = dto.Email;
			}
			customer.UpdatedAt = DateTime.Now;
			_context.Entry(customer).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return Ok(new { Message = "Cập nhật thành công!", Data = customer });
		}
		//Helper
		private UserCustomer ConvertDTOToEntity(CustomerCreateReqDto dto)
		{
			var newUser = new UserCustomer
			{	
				Name = dto.Name,
				Phone = dto.Phone,
				Email = dto.Email,
				UserStatusId = dto.UserStatusId,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				RowId = Guid.NewGuid(),
				UserTypeId = 1,
			};
			return newUser;
		}
		private IQueryable<UserCustomer> ConvertFilterDTOToFilterEntity(CustomerFilterDto filterDto)
		{
			var filter = _context.UserCustomers.AsQueryable();
			if (!string.IsNullOrEmpty(filterDto.Name))
			{
				filter = filter.Where(x => x.Name.Contains(filterDto.Name));
			}
			if (!string.IsNullOrEmpty(filterDto.Email))
			{
				filter = filter.Where(x => x.Email.Contains(filterDto.Email));
			}
			if(!string.IsNullOrEmpty(filterDto.Phone))
			{
				filter = filter.Where(x => x.Phone == filterDto.Phone);
			}
			return filter;
		}
	}
}