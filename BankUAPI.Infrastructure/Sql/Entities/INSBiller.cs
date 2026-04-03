using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Infrastructure.Sql.Entities
{
    public class INSBiller
    {
        public int Id { get; set; }

        public string? categoryKey { get; set; } 

        public string? categoryName { get; set; }

        public string? iconUrl { get; set; }
    }
}
