using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.BLL.Services;
using TestSystem.Core.Interfaces;

namespace TestSystem.Core.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private UserService userService = new UserService();
        private IUserInformationService informationService;
        private string login;
        private string password;
        public bool IsAuntificated { get; private set; } = false;

        public void CheckAuthorization(string login, string password)
        {
            informationService = new UserInformationService(userService);
            IsAuntificated = userService.CheckAuthorization(login, password);
            if (IsAuntificated == true)
            {
                informationService.GetUserInfo(login);
                this.login = login;
                this.password = login;
            }
        }

        public bool CreateAuthorization(string login, string password)
        {
           IsAuntificated = userService.CreateUser(login, password, new BLL.DTO.User());
            if(this.IsAuntificated)
            {
                this.login = login;
                this.password = password;
            }
            return IsAuntificated;
        }

        public IUserInformationService GetUserInformation()
        {
            if (login == "")
                return null;
            return informationService;
        }

        public void LogOut()
        {
            IsAuntificated = false;
            login = password = "";
            informationService = new UserInformationService(userService);
        }
    }
}
