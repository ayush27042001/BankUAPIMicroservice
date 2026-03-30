using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Infrastructure.Sql.Entities
{
    public class tblcircle
    {
        public int Id { get; set; }

        public string? CircleCode { get; set; }

        public string? CircleName { get; set; } = null!;
        public string? INSPAYCircle { get; set; } = null!;
    }
}
