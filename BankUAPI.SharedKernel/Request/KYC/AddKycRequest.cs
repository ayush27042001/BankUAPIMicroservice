using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.KYC
{
    public class AddKycRequest
    {
        public string RegistrationId { get; set; }

        public IFormFile AadharUpload { get; set; }

        public IFormFile PanUpload { get; set; }

        public IFormFile PhotoUpload { get; set; }

        public IFormFile GstUpload { get; set; }

        public IFormFile ShopFrontUpload { get; set; }

        public IFormFile ShopInUpload { get; set; }

        public IFormFile KycApplication { get; set; }

        public string BusinessProofUploadtype { get; set; }
    }
}
