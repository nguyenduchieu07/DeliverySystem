using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Helpers
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class Hashor
    {

        public static string ToBase64(string input)
            => Convert.ToBase64String(Encoding.UTF8.GetBytes(input));

      
        public static string FromBase64(string base64)
            => Encoding.UTF8.GetString(Convert.FromBase64String(base64));

    }

}
