using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestSystem.BLL.DTO;
using TestSystem.BLL.Services;
using TestSystem.Core.Interfaces;

namespace TestSystem.Core.Services
{
    public class UserInformationService : IUserInformationService
    {
        private readonly UserService userService;
        private User user;
        public bool IsAdmin { get; private set; }
        public UserInformationService(UserService userService)
        {
            this.userService = userService;
        }

        public User UserInformation { get => user; set => user = value; }

        public void EditUser(User editInformation)
        {
            userService.UpdateInformation(editInformation);
            userService.UpdatePhoto(editInformation);
            user = editInformation;
        }

        public void GetUserInfo(string login)
        {
            user = userService.GetUser(login);
            IsAdmin = userService.IsAdmin(login);
        }
    }
}
