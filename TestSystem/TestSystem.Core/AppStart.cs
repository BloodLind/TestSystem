using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.Core.ViewModels;
using MvvmCross;
using MvvmCross.Navigation;
using TestSystem.Core.Interfaces;
using TestSystem.Core.Services;
using TestSystem.BLL.Services;

namespace TestSystem.Core
{
    public class AppStart : MvxApplication
    {
        public AppStart()
        {
            
        }
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterType<TestService, TestService>();
            Mvx.IoCProvider.RegisterType<InformationService, InformationService>();
            this.RegisterAppStart<CatalogViewModel>();   
        }

        public static IAuthorizationService AuthorizationService { get; set; } = new AuthorizationService();
    }
}
