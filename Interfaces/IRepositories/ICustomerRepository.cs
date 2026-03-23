using MyApi.Models;
using MyApi.DTOs;

namespace MyApi.Interfaces;
public interface ICustomerRepository
{
  Task<UserCustomer?> Get(long id);
  Task<List<CustomerGetResDto>?> List(CustomerFilterDto query);
  Task<UserCustomer> Create(UserCustomer newCustomer);
  Task<UserCustomer?> Update(long id,  CustomerUpdateReqDto dto);
  //Validator
  Task<bool> IsPhoneExisted(string phone);
}