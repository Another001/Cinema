using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Interfaces;
public interface ICustomerService : IServiceScoped
{
    Task<CustomerGetResDto?> Get(long id);
    Task<List<CustomerGetResDto>?> List(CustomerFilterDto dto);
    Task<UserCustomer> Create(CustomerCreateReqDto dto);
    Task<UserCustomer> Update(long id, CustomerUpdateReqDto dto);
}