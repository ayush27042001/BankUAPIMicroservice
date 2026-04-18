using BankUAPI.SharedKernel.Request.ZohoMailSent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.ZohoMailService
{
    public interface IRiskService
    {
        Task<RiskResult> EvaluateRisk(string email, string deviceId, string ip);
        Task SaveTrustedDevice(string email, string deviceId, string ip);
    }
}
