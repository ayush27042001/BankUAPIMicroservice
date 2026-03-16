using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Infrastructure.Sql.Entities
{
    public class UserBanks
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        public string? AccountNo { get; set; }

        public string? IFSC { get; set; }

        public string? BankName { get; set; }

        public string? AccHolder { get; set; }
    }
}
