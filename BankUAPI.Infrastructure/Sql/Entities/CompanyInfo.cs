using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class CompanyInfo
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? CompanyNumber { get; set; }

    public string? Email { get; set; }

    public string? Location { get; set; }

    public string? LogoImage { get; set; }

    public string? AboutUs { get; set; }

    public string? Facebook { get; set; }

    public string? Twitter { get; set; }

    public string? Instagram { get; set; }

    public string? LinkedIn { get; set; }

    public string? GalleryImage1 { get; set; }

    public string? GalleryImage2 { get; set; }

    public string? GalleryImage3 { get; set; }

    public string? GalleryImage4 { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? UserId { get; set; }

    public string? Status { get; set; }
}
