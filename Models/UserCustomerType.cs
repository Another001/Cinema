using System;
using System.Collections.Generic;

namespace MyApi.Models;

public partial class UserCustomerType
{
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? Color { get; set; }

    public virtual ICollection<UserCustomer> UserCustomers { get; set; } = new List<UserCustomer>();
}
