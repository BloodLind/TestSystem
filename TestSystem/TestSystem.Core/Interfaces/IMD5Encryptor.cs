using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace TestSystem.Core.Interfaces
{
    public interface IMD5Encryptor
    {
        string GenerateMD5Hesh(string inputData);
    }
}
