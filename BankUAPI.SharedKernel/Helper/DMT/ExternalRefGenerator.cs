using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Helper.DMT
{
    public static class ExternalRefGenerator
    {
        private static int _counter = 0;

        public static string Generate()
        {
            long epochMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            int count = Interlocked.Increment(ref _counter) & 0xFFF;

            return $"{epochMs}{count:D4}";
        }
    }
}
