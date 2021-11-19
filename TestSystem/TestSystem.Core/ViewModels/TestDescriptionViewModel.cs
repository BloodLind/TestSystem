using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestSystem.BLL.DTO;
using TestSystem.BLL.Services;
using TestSystem.Core.Models;

namespace TestSystem.Core.ViewModels
{
    public class TestDescriptionViewModel : MvxViewModel
    {
        private readonly InformationService informationService;
        private readonly IMvxNavigationService mvxNavigation;
        private TestsMaterial testMaterial;
        private readonly TestModel test;
        private bool isEditing = false;
        private string materialName;
        private string materialInfo;
        private bool isAdmin;
        private bool canSave;

        public TestDescriptionViewModel(IMvxNavigationService mvxNavigation, 
            InformationService informationService,
            TestModel test)
        {
            this.informationService = informationService;
            this.mvxNavigation = mvxNavigation;
            this.test = test;
            InititalizateCommands(); 
            InititializateInformation();
            if (AppStart.AuthorizationService.IsAuntificated)
                IsAdmin = AppStart.AuthorizationService.GetUserInformation().IsAdmin;
        }

        private void InititializateInformation()
        {
            testMaterial = informationService.GetTestsMaterials().FirstOrDefault(x =>
               x.DisciplineId == test.DisciplineId &&
               x.ThemeId == test.ThemeId);
            testMaterial = testMaterial == null ? new TestsMaterial() : testMaterial;
            TestMaterial = testMaterial.Name;
            MaterialInfo = testMaterial.Information;
            testMaterial.ThemeId = test.ThemeId;
            testMaterial.LanguageId = test.LanguageId;
            testMaterial.DisciplineId = test.DisciplineId;
        }

        private void InititalizateCommands()
        {
            CloseCommand = new MvxCommand(() => mvxNavigation.Close(this));
            EditCommand = new MvxCommand(() => 
            {
                CanSave = true;
                IsReadOnly = false;
            });
            SaveChangesCommand = new MvxCommand(() =>
            {
                testMaterial.Name = materialName;
                testMaterial.Information = materialInfo;
                
                informationService.AddOrUpdateTestMaterial(testMaterial);
                IsReadOnly = true;
                CanSave = false;
            });
            StartTestCommand = new MvxCommand(() =>
            {
                mvxNavigation.Navigate(new PassTestViewModel(mvxNavigation, 
                    new TestService().GetTests().First(x => x.Id == test.Id)));
            });
        }

        
        public string MaterialInfo { get => materialInfo; set { materialInfo = value; RaisePropertyChanged(() => MaterialInfo); } }
        public string TestTitle { get => test?.Name; }
        public string TestMaterial { get => materialName; set { materialName = value; RaisePropertyChanged(() => TestMaterial); } }
        public string Language { get => test?.Language; }
        public string Theme { get => test?.Theme; }
        public string Discipline { get => test?.Discipline; }

        public bool CanSave { get => canSave; set { canSave = value; RaisePropertyChanged(() => CanSave); } }
        public bool IsReadOnly { get => isEditing; set { isEditing = value; RaisePropertyChanged(() => IsReadOnly); } }

        public bool IsAdmin { get => isAdmin; set { isAdmin = value;RaisePropertyChanged(() => IsAdmin); } }
        #region Commands
        public IMvxCommand SaveChangesCommand { get; private set; }
        public IMvxCommand StartTestCommand { get; private set; }
        public IMvxCommand CloseCommand { get; private set; }
        public IMvxCommand EditCommand { get; private set; }
        #endregion
    }
}
