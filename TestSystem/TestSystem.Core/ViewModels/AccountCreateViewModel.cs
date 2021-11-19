using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TestSystem.Core.Services;

namespace TestSystem.Core.ViewModels
{
    public class AccountCreateViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService mvxNavigation;
        private string login;
        private int loadValue;
        private bool isIndeterminate = true;

        public AccountCreateViewModel(IMvxNavigationService mvxNavigation)
            
        {
            InitializateCommands();
            this.mvxNavigation = mvxNavigation;
        }

        private void InitializateCommands()
        {
            CloseCommand = new MvxCommand(() => mvxNavigation.Close(this));
            LoginCommand = new MvxAsyncCommand(() => Task.Run(Authorizate));
            CreateNewCommand = new MvxCommand(() =>
            {
                mvxNavigation.Navigate(new CreateAccountAllDataViewModel(mvxNavigation));
                mvxNavigation.AfterClose += MvxNavigation_AfterClose;
                
                    });
        }

        private void MvxNavigation_AfterClose(object sender, MvvmCross.Navigation.EventArguments.IMvxNavigateEventArgs e)
        {
            mvxNavigation.AfterClose -= MvxNavigation_AfterClose;
            if (AppStart.AuthorizationService.IsAuntificated)
                mvxNavigation.Close(this);
        }

        private void Authorizate()
        {
            
            Password = new MD5EncryptorService().GenerateMD5Hesh(Password);
            AppStart.AuthorizationService.CheckAuthorization(Login, Password);
            if (AppStart.AuthorizationService.IsAuntificated)
            {
                IsIndeterminate = false;
                Task.Run(() =>
              {
                  while (LoadValue++ <= 100) Thread.Sleep(5) ;
                  Thread.Sleep(75);
                  mvxNavigation.Close(this);
              });
            }
            else
                Login = "Wrong Password or Login!";
        }

        public bool IsIndeterminate { get => isIndeterminate; set { isIndeterminate = value; RaisePropertyChanged(() => IsIndeterminate); } }
        public int LoadValue { get => loadValue; set { loadValue = value; RaisePropertyChanged(() => LoadValue); } }
        public string Login { get => login; set { login = value; RaisePropertyChanged(() => Login); } }
        public string Password { get; set; }

        #region Commands
        public ICommand CreateNewCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }
        public ICommand LoginCommand { get; private set; }
        #endregion
    }
}
