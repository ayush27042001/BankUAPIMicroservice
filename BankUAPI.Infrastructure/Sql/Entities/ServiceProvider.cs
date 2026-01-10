using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Infrastructure.Sql.Entities
{
    public class ServiceProvider
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string ServiceCode { get; set; } = null!;

        [Required, MaxLength(50)]
        public string ProviderCode { get; set; } = null!;

        public bool IsEnabled { get; set; }

        // Navigation
        [ForeignKey(nameof(ProviderCode))]
        public MasterProvider Provider { get; set; } = null!;
        public ICollection<ServiceProviderFeatureMap> FeatureMaps { get; set; } = new List<ServiceProviderFeatureMap>();
    }
}
