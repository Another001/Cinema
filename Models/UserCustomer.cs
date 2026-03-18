using System;
using System.Collections.Generic;

namespace MyApi.Models;

public partial class UserCustomer
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public long UserStatusId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid RowId { get; set; }

    public long UserTypeId { get; set; }

    public virtual ICollection<BookingReservation> BookingReservations { get; set; } = new List<BookingReservation>();

    public virtual UserCustomerStatus UserStatus { get; set; } = null!;

    public virtual UserCustomerType UserType { get; set; } = null!;
}
