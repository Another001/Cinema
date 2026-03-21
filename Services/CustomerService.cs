using MyApi.Repositories;
using MyApi.DTOs;
using MyApi.Interfaces;
using MyApi.Models;

namespace MyApi.Services;
public class CustomerService : ICustomerService
{
  private readonly ICustomerRepository _userRepo;

  public CustomerService(ICustomerRepository userRepo) => _userRepo = userRepo;

  public async Task<CustomerGetResDto?> Get(long id)
    {
    var user = await _userRepo.Get(id);
    if (user == null) return null;

    // Mapping thủ công từ Entity sang DTO
    return new CustomerGetResDto {
        Name = user.Name,
        Email = user.Email,
        Phone = user.Phone,
        Status = user.UserStatus.Name
    };
  }
  public async Task<List<CustomerGetResDto>?> List(CustomerFilterDto dto)
  {
    var customers = await _userRepo.List(dto);
    return customers;
  }
  public async Task<UserCustomer> Create(CustomerCreateReqDto dto)
  {
    var newCustomer = ConvertDTOToEntity(dto);
    await _userRepo.Create(newCustomer);
    return newCustomer;
  }
  public async Task<UserCustomer> Update(long id, CustomerUpdateReqDto dto)
  {
    try
    {
      var customer = await _userRepo.Update(id, dto);
      return customer;
    }
    catch
    {
      throw; 
    }
  }
  //Helper
  private UserCustomer ConvertDTOToEntity(CustomerCreateReqDto dto)
  {
    var customer = new UserCustomer
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
    return customer;
  }
}