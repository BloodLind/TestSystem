using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.Core.Interfaces
{
    public interface IAuthorizationService
    {
        void CheckAuthorization(string login, string password);
        bool IsAuntificated { get; }
        bool CreateAuthorization(string login, string password);
        IUserInformationService GetUserInformation();
        void LogOut();
    }
}
