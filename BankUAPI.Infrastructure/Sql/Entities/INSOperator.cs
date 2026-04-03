using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Infrastructure.Sql.Entities
{
    public class  INSOperator
    {
        [Key]
        public string? billerId { get; set; }
        public string? billerName { get; set; }

        public string? categoryKey { get; set; }

        public string? categoryName { get; set; }

        public string? iconUrl { get; set; }
    }
}
