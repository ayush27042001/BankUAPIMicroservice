using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class Apidocument
{
    public int Id { get; set; }

    public string? Apiname { get; set; }

    public string? Link { get; set; }

    public string? Category { get; set; }

    public string? Discription { get; set; }

    public string? HeaderPara { get; set; }

    public string? RequestPara { get; set; }

    public string? SampleReq { get; set; }

    public string? ResponsePara { get; set; }

    public string? SampleResponse { get; set; }

    public string? Status { get; set; }

    public string? ApiType { get; set; }
}
