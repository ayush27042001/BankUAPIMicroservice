using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class AddContact
{
    public int Id { get; set; }

    public string? ContactType { get; set; }

    public string? ContactPersonName { get; set; }

    public string? CompanyName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? UserId { get; set; }

    public string? Pan { get; set; }

    public string? Cin { get; set; }

    public string? Gstin { get; set; }

    public string? Tan { get; set; }

    public string? Udyam { get; set; }

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Pincode { get; set; }
}
