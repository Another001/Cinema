using MyApi.DTOs;
using MyApi.Interfaces;
using MyApi.Models;

namespace MyApi.Services;
public class CustomerService : ICustomerService
{
  private readonly ICustomerRepository _userRepo;

  public CustomerService(ICustomerRepository userRepo) { _userRepo = userRepo;}

  public async Task<CustomerGetResDto?> Get(long id)
    {
    var user = await _userRepo.Get(id);
    if (user == null) 
      throw new KeyNotFoundException($"Không tìm thấy khách hàng với ID: {id}");
    
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
    if (string.IsNullOrEmpty(dto.Name))
    {
      throw new Exception("Ten khong duoc de trong");
    }
    if (string.IsNullOrEmpty(dto.Email))
    {
      throw new Exception("Email khong duoc de trong");
    }
    if (string.IsNullOrEmpty(dto.Phone))
    {
      throw new Exception("So dien thoai khong duoc de trong");
    }
    if (dto.Phone.Length < 3)
    {
      throw new Exception("So dien thoai khong hop le");
    }
    var isUsedPhone = await _userRepo.IsPhoneExisted(dto.Phone);
    if (isUsedPhone)
    {
      throw new Exception("So dien thoai da duoc su dung");
    }
    var newCustomer = ConvertDTOToEntity(dto);
    await _userRepo.Create(newCustomer);
    return newCustomer;
  }
  public async Task<UserCustomer> Update(long id, CustomerUpdateReqDto dto)
  {
    try
    {
      var customer = await _userRepo.Update(id, dto);
      if(customer == null)
        throw new Exception("Khong tim thay nguoi dung");
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