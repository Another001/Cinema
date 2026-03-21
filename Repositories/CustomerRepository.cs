using Microsoft.EntityFrameworkCore;
using MyApi.Models;
using MyApi.Interfaces;
using MyApi.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MyApi.Repositories;
public class CustomerRepository : ICustomerRepository
{
  private readonly TestContext _context;
  public CustomerRepository(TestContext context) => _context = context;

  public async Task<UserCustomer?> Get(long id)
  {
    return await _context.UserCustomers
      .Include(u => u.UserStatus)
      .FirstOrDefaultAsync(u => u.Id == id && u.DeletedAt == null);
  }
  public async Task<List<CustomerGetResDto>?> List(CustomerFilterDto dto)
  {
    var query = ConvertFilterDtoToFilterEntity(dto);
    var customers = await (
      from customer in query
      join status in _context.UserCustomerStatuses
      on customer.UserStatusId equals status.Id
      select new CustomerGetResDto
      {
        Name = customer.Name,
        Email = customer.Email,
        Phone = customer.Phone,
        Status = status.Code
      }
    ).ToListAsync();
    return customers;
  }
  public async Task<UserCustomer> Create(UserCustomer newCustomer)
  {
    _context.UserCustomers.Add(newCustomer);
    await _context.SaveChangesAsync();
    return newCustomer;
  }
  public async Task<UserCustomer> Update (long id, CustomerUpdateReqDto dto)
  {
    var customer = _context.UserCustomers.Find(id);
    if(customer == null)
    {
      throw new Exception("Khong ton tai user");
    };
    customer.Name = dto.Name ?? customer.Name;
    customer.Email = dto.Email ?? customer.Email;
    customer.UpdatedAt = DateTime.Now;
    await _context.SaveChangesAsync();
    return customer;
  }
  //Helper
  public IQueryable<UserCustomer> ConvertFilterDtoToFilterEntity(CustomerFilterDto dto)
  {
    var query = _context.UserCustomers.AsQueryable();
    if (!string.IsNullOrEmpty(dto.Name))
    {
      query = query.Where(x => x.Name == dto.Name);
    }
    if (!string.IsNullOrEmpty(dto.Email))
    {
      query = query.Where(x => x.Email == dto.Email);
    }if (!string.IsNullOrEmpty(dto.Phone))
    {
      query = query.Where(x => x.Phone == dto.Phone);
    }
    return query;
  }
}