using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TestSystem.BLL.DTO;
using TestSystem.BLL.Services;
using TestSystem.Core.Interfaces;

namespace TestSystem.Core.ViewModels
{
    public class AccountViewModel : MvxViewModel
    {
        
        private IUserInformationService userInformationService;
        private readonly IMvxNavigationService mvxNavigation;
        private readonly TestService testService;
        private bool isReadOnly = true;
        private string secondname = "";
        private string fathername = "";
        private string firstname = "";
        

        public AccountViewModel(IMvxNavigationService mvxNavigation, BLL.Services.TestService testService)
        {
            Results = new MvxObservableCollection<Result>();
            this.mvxNavigation = mvxNavigation;
            this.testService = testService;
            InitializateCommands();
        }


        public override void ViewAppeared()
        {
            if (!AppStart.AuthorizationService.IsAuntificated)
                NavigateToCreateAccount();
            else
            {
                AfterCreating();
            }
        }


        private void InitializateCommands()
        {
            LogOut = new MvxCommand(() => { AppStart.AuthorizationService.LogOut(); mvxNavigation.Close(this); });
            CloseCommand = new MvxCommand(() => mvxNavigation.Close(this));
            EditUserInfo = new MvxCommand(() => EditUser());
        }
        private void EditUser()
        {
            if(isReadOnly)
            {
                IsEnabled = false;
                return;
            }
            if (firstname != "" && secondname != "" && fathername != "")
            {
                IsEnabled = true;
                User edit = new User
                {
                    Id = userInformationService.UserInformation.Id,
                    Secondname = secondname,
                    Fathername = fathername,
                    Firstname = firstname
                };
                userInformationService.EditUser(edit);
            }

        }
       
        private void NavigateToCreateAccount()
        {
            mvxNavigation.Navigate(new AccountCreateViewModel(mvxNavigation));
            mvxNavigation.AfterClose += NavigationService_AfterClose; 

        }

        private void NavigationService_AfterClose(object sender, MvvmCross.Navigation.EventArguments.IMvxNavigateEventArgs e)
        {
            mvxNavigation.AfterClose -= NavigationService_AfterClose;
            AfterCreating();
        }

        private void AfterCreating()
        {
            if (!AppStart.AuthorizationService.IsAuntificated)
            {
                mvxNavigation.Close(this);
                return;
            }
            userInformationService = AppStart.AuthorizationService.GetUserInformation();
            Secondname = userInformationService.UserInformation.Secondname;
            Fathername = userInformationService.UserInformation.Fathername;
            Firstname = userInformationService.UserInformation.Firstname;

            var results = testService.GetResults().FindAll((x) => x.UserId == userInformationService.UserInformation.Id);
            foreach (var result in results)
            {
                Results.Add(result);
            }
        }

        public string Firstname { 
            get => firstname; set 
            {
                firstname = value;
                RaisePropertyChanged(() => Firstname);
            } 
        }
        public MvxObservableCollection<Result> Results { get; set; }
        public string Secondname 
        {
            get => secondname;
            set
            {
                secondname = value;
                RaisePropertyChanged(() => Secondname);
            }
        }
        public string Fathername 
        {
            get => fathername;
            set
            {
                fathername = value;
                RaisePropertyChanged(() => Fathername);
            }
        }
        public bool IsReadOnly { get => isReadOnly; set
            {
                isReadOnly = value;
                RaisePropertyChanged(() => IsReadOnly);
            } }
        public bool IsEnabled { get => !IsReadOnly; set { IsReadOnly = value; RaisePropertyChanged(() => IsReadOnly); } }
        
        #region Commands
        public ICommand CloseCommand { get; private set; }
        public ICommand EditUserInfo { get; private set; }
        public ICommand LogOut { get; private set; }
        #endregion
    }
}
