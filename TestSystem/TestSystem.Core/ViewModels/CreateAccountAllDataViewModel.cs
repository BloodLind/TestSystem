using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestSystem.BLL.DTO;
using TestSystem.Core.Services;

namespace TestSystem.Core.ViewModels
{
    public class CreateAccountAllDataViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService mvxNavigation;
        private string fathername = "Oh and fathername too!";
        private string secondname = "And secondname too";
        private string firstname = "Your beautiful name";
        private bool isIndeterminate = true;
        private int loadValue = 0;
        private string login;


        public CreateAccountAllDataViewModel(IMvxNavigationService mvxNavigation)
        {
            this.mvxNavigation = mvxNavigation;
            InitializateCommands();
        }
        private void InitializateCommands()
        {
            CreateAccountCommand = new MvxAsyncCommand(() => Task.Run(Authorizate));
            CloseCommand = new MvxCommand(() => mvxNavigation.Close(this));
        }

        private void Authorizate()
        {
            if(Password.Length <= 3 || Login.Length <= 3)
            {
                Login = "Password and Login must have 4 characters";
                return;
            }
            else if(login.Contains(" "))
            {
                Login = "You need to delete spaces! NOW!";
                return;
            }
            var encryptor = new MD5EncryptorService();
            Password = encryptor.GenerateMD5Hesh(Password);
            if(!AppStart.AuthorizationService.CreateAuthorization(Login, Password))
            {
                Login = "Login is used! Choose another one.";
                return;
            }
            
            var userService = AppStart.AuthorizationService.GetUserInformation();
            
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Properties.Resources.no_image.Save(memoryStream, Properties.Resources.no_image.RawFormat);
                userService.EditUser(new User()
                {
                    Id = userService.UserInformation.Id,
                    Photo = memoryStream.ToArray(),
                    Fathername = fathername,
                    Secondname = secondname,
                    Firstname = firstname
                }) ;
            }
            IsIndeterminate = false;
            Task.Run(() =>
            {
                while (LoadValue++ <= 100) Thread.Sleep(5);
                Thread.Sleep(75);
                mvxNavigation.Close(this);
            });
        }

        public bool IsIndeterminate { get => isIndeterminate; set { isIndeterminate = value; RaisePropertyChanged(() => IsIndeterminate); } }
        public string Fathername { get => fathername; set { fathername = value; RaisePropertyChanged(() => Fathername); } }
        public string Secondname { get => secondname; set { secondname = value; RaisePropertyChanged(() => Secondname); } }
        public string Firstname { get => firstname; set { firstname = value; RaisePropertyChanged(() => Firstname); } }
        public int LoadValue { get => loadValue; set { loadValue = value; RaisePropertyChanged(() => LoadValue); } }
        public string Login { get => login; set { login = value; RaisePropertyChanged(() => Login); } }
        public string Password { get; set; } = "";


        

        #region Commands
        public IMvxCommand CreateAccountCommand { get; private set; }
        public IMvxCommand CloseCommand { get; private set; }
        #endregion
    }
}
