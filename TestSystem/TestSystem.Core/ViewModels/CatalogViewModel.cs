using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TestSystem.BLL.DTO;
using TestSystem.BLL.Services;
using TestSystem.Core.Models;

namespace TestSystem.Core.ViewModels
{
    public class CatalogViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService mvxNavigation;
        private readonly InformationService infoService;
        private readonly TestService testService;
        private List<TestModel> testModels;
        private TestModel selectedTest;
        private bool isAdmin;

        public CatalogViewModel(IMvxNavigationService mvxNavigation, TestService testService, InformationService service)
        {
            Tests = new MvxObservableCollection<TestModel>();
            this.mvxNavigation = mvxNavigation;
            this.testService = testService;
            this.infoService = service;
            InitializateCommands();
            Task.Run(InitializateTests);
        }

        private void InitializateTests()
        {
            var disciplines = infoService.GetDisciplines();
            var languages = infoService.GetLanguages();
            var themes = infoService.GetThemes();
            var tests = testService.GetTests();
            mvxNavigation.AfterClose += MvxNavigation_AfterClose;
            Disciplines = new MvxObservableCollection<Discipline>(disciplines);
            SelectedDiscipline = disciplines.ElementAt(0);
            testModels = new List<TestModel>();

            foreach(var test in tests)
            {
                TestModel testModel = new TestModel
                {
                    DisciplineId = Convert.ToInt32(test.DisciplineId),
                    LanguageId = Convert.ToInt32(test.LanguageId),
                    ThemeId = Convert.ToInt32(test.ThemeId),
                    Name = test.Name,
                    Id = test.Id
                };
                foreach(var discipline in disciplines)
                {
                    if (discipline.Id == testModel.DisciplineId)
                        testModel.Discipline = discipline.Name;
                }

                foreach(var theme in themes)
                {
                    if (theme.Id == testModel.ThemeId)
                        testModel.Theme = theme.Name;
                }

                foreach(var language in languages)
                {
                    if (language.Id == testModel.LanguageId)
                        testModel.Language = language.Name;
                }

                testModels.Add(testModel);
                Tests.Add(testModel);
            }
        }

        private void ApplyFilter()
        {
                Tests.Clear();
                if (Filters.Count == 0)
                {
                    foreach(var test in testModels)
                    {
                        Tests.Add(test);
                    }
                }
                else
                {
                    foreach (var test in testModels)
                    {
                        if (Filters.Any(x => x.Id == test.DisciplineId))
                        {
                            Tests.Add(test);
                        }
                    }
                }
        }
        private void InitializateCommands()
        {
            AccountNavigateCommand = new MvxCommand(() => { 
                mvxNavigation.Navigate(new AccountViewModel(mvxNavigation, testService)); 
                 });
            CreateTestCommand = new MvxCommand(() => 
            {
                CreateTestViewModel vm = new CreateTestViewModel(mvxNavigation, testService, infoService);
                vm.AfterTestAdded += Vm_AfterTestAdded;
                mvxNavigation.Navigate(vm);
            });

            RemoveFilterCommand = new MvxAsyncCommand(() => Task.Run(() => 
            { 
                Filters.Remove(SelectedFilter);
                ApplyFilter();
            }));
            AddFilterCommand = new MvxAsyncCommand(() => Task.Run(() =>
            {
                if (!Filters.Contains(SelectedDiscipline))
                {
                   Filters.Add(SelectedDiscipline);
                   ApplyFilter();
                }
            }));
            ShowDescription = new MvxCommand(() =>
            {
                mvxNavigation.Navigate(new TestDescriptionViewModel(mvxNavigation, infoService, SelectedTest));
            });
            RemoveTestCommand = new MvxCommand(() =>
            {
                testService.DeleteTest(selectedTest.Id);
                Tests.Remove(SelectedTest);
                SelectedTest = Tests.Count > 0 ? Tests[0] : null;
            });
        }

        private void Vm_AfterTestAdded()
        {

                    
            var tests = testService.GetTests();
            var test = tests[tests.Count - 1];
            if(Tests.Last(x => x.Id != test.Id) != null)
            {
                Tests.Add(new TestModel
                { 
                    Name = test.Name,
                    Language = infoService.GetLanguages().First(x => x.Id == test.LanguageId)?.Name,
                    Discipline = infoService.GetDisciplines().First(x => x.Id == test.DisciplineId)?.Name,
                    Theme = infoService.GetThemes().First(x => x.Id == test.DisciplineId)?.Name,
                    Id = test.Id
                });
            }
        }

        private void MvxNavigation_AfterClose(object sender, MvvmCross.Navigation.EventArguments.IMvxNavigateEventArgs e)
        { 
            if (AppStart.AuthorizationService.IsAuntificated)
            {
                IsAdmin = AppStart.AuthorizationService.GetUserInformation().IsAdmin;
            }
            else
                IsAdmin = false;
        }


        public bool IsAdmin { get => isAdmin; private set { isAdmin = value; RaisePropertyChanged(() => IsAdmin); } }
        
       
        public MvxObservableCollection<Discipline> Disciplines { get; set; }
        public Discipline SelectedDiscipline { get; set; }


        public TestModel SelectedTest { get => selectedTest; set { selectedTest = value; RaisePropertyChanged(() => SelectedTest); } }
        public MvxObservableCollection<Discipline> Filters { get; set; } = new MvxObservableCollection<Discipline>();
        public MvxObservableCollection<TestModel> Tests { get; set; }
        public Discipline SelectedFilter { get; set; }
        
        
        #region Commands
        public IMvxCommand AccountNavigateCommand { get; private set; }
        public IMvxCommand RemoveFilterCommand { get; private set; }
        public IMvxCommand CreateTestCommand { get; private set; }
        public IMvxCommand AddFilterCommand { get; private set; }
        public IMvxCommand ShowDescription { get; private set; }
        public IMvxCommand RemoveTestCommand { get; private set; }
        #endregion
    }
}
