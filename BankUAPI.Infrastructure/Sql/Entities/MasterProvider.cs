using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Infrastructure.Sql.Entities
{
    public class MasterProvider
    {
        [Key]
        public int ProviderId { get; set; }

        [Required, MaxLength(50)]
        public string ProviderCode { get; set; } = null!;

        [Required, MaxLength(150)]
        public string ProviderName { get; set; } = null!;

        public bool IsEnabled { get; set; }

        // Navigation
        public ICollection<ServiceProvider> ServiceProviders { get; set; } = new List<ServiceProvider>();
        public ICollection<ServiceProviderFeatureMap> ServiceProviderFeatureMaps { get; set; } = new List<ServiceProviderFeatureMap>();
    }
}
