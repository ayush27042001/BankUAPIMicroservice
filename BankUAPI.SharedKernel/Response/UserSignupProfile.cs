using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response
{
    public sealed class UserSignupProfile
    {
        public string UserId { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
    }

}
