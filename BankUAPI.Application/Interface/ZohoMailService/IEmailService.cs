using BankUAPI.SharedKernel.Request.ZohoMailSent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.ZohoMailService
{
    public interface IEmailService
    {
        Task SendOtpEmail(EmailRequest request);
    }
}
