using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Infrastructure.Sql.Entities
{
    public partial class UserDevices
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? Email { get; set; }
        public string? DeviceId { get; set; }
        public string? IpAddress { get; set; }
        public DateTime? LastUsed { get; set; }
        public bool? IsTrusted { get; set; }

    }
}
