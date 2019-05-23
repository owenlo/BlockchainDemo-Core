using System;
using System.Text;

namespace BlockchainDemo.Helpers
{
    public static class ConvertTo
    {
        //Source: https://stackoverflow.com/questions/2435695/converting-a-md5-hash-byte-array-to-a-string
        public static string Hex(this byte[] bytes, bool upperCase)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }
    }
}
