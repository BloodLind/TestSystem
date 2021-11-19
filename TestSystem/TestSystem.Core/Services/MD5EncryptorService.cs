using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TestSystem.Core.Interfaces;

namespace TestSystem.Core.Services
{
    public class MD5EncryptorService : IMD5Encryptor
    {
        private MD5 encryptor = MD5.Create();
        public string GenerateMD5Hesh(string inputData)
        {
            return Convert.ToBase64String(encryptor.ComputeHash(Encoding.UTF8.GetBytes(inputData)));
        }
    }
}
