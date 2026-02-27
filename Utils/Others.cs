using System;
using System.Security.Cryptography;

namespace MultiOperationExecutioner.Utils
{

    public static class Others
    {
        public static string RandomHashGenerate(int byteLength = 16)
        {
            byte[] bytes = new byte[byteLength];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToHexString(bytes);
        }



        
    }
}
