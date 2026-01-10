using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Infrastructure.Sql.Entities
{
    public class MasterFeature
    {
        [Key]
        public int FeatureId { get; set; }

        [Required, MaxLength(50)]
        public string ServiceCode { get; set; } = null!;

        [Required, MaxLength(50)]
        public string FeatureCode { get; set; } = null!;

        [Required, MaxLength(150)]
        public string FeatureName { get; set; } = null!;

        [MaxLength(50)]
        public string? Icon { get; set; }

        public string? ExtraConfig { get; set; }

        public bool IsEnabled { get; set; }

        public int DisplayOrder { get; set; }

        // Navigation
        public ICollection<ServiceProviderFeatureMap> ServiceProviderFeatureMaps { get; set; } = new List<ServiceProviderFeatureMap>();
    }
}
