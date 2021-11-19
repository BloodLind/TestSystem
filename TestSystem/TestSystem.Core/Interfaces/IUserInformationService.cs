using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.BLL.DTO;

namespace TestSystem.Core.Interfaces
{
    public interface IUserInformationService
    {
        bool IsAdmin { get; }
        User UserInformation { get; set; }
        void EditUser(User editInformation);
        void GetUserInfo(string login);
    }
}
