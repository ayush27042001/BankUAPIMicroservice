using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.Payout.IDFC
{
    public sealed class IDFCApiResponse<T>
    {
        public string status { get; init; } = "SUCCESS";
        public string message { get; init; } = "SUCCESS";
        public T? data { get; init; }

        public static IDFCApiResponse<T> Ok(T data)
            => new() { data = data };

        public static IDFCApiResponse<T> Fail(string message)
            => new() { status = "FAILED", message = message };
    }
}
