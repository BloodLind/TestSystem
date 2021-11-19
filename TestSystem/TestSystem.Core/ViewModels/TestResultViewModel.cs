using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.BLL.DTO;
using TestSystem.BLL.Services;

namespace TestSystem.Core.ViewModels
{
    class ResultData
    {
        public Question Question { get; set; }
        public Answer SelectedAnswer { get; set; }
    }
    class TestResultViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService mvxNavigation;
        private readonly Answer[] selectedAnswers;
        private readonly Test currentTest;
        private string testName;
        private int points;
        private int percent;

        public TestResultViewModel(IMvxNavigationService mvxNavigation, 
            Answer[] selectedAnswers, 
            Test currentTest)
        {
            this.mvxNavigation = mvxNavigation;
            this.selectedAnswers = selectedAnswers;
            this.currentTest = currentTest;
            TestName = currentTest.Name;
            InitializateCommands();
            Task.Run(SaveResult);
            Task.Run(() => InitializateCollections(currentTest.Questions, selectedAnswers));
        }

        private void InitializateCollections(IEnumerable<Question> questions, IEnumerable<Answer> answers)
        {
            QuestionsWithAnswers = new MvxObservableCollection<ResultData>();
            int i = 0;
            foreach(var question in questions)
            {
                QuestionsWithAnswers.Add(new ResultData() { Question = question, SelectedAnswer = selectedAnswers[i++]});
                
            }
        }
        private void SaveResult()
        {
            Result result = new Result();
            if(AppStart.AuthorizationService.IsAuntificated)
            {
                result.UserId = AppStart.AuthorizationService.GetUserInformation().UserInformation.Id;
            }
            result.TestId = currentTest.Id;
            int correctAnswers = 0;
           
            foreach (var question in currentTest.Questions)
                correctAnswers += Convert.ToInt32(selectedAnswers.Contains(question.CorrectAnswer));

            CorrectPercent = result.CorrectPercent = (correctAnswers / currentTest.Questions.Count) * 100;
            Points = result.Points = correctAnswers;
            var service = new TestService();
            service.AddResult(result);
        }
        private void InitializateCommands()
        {
            CloseCommand = new MvxCommand(() => mvxNavigation.Close(this));
        }

        public MvxObservableCollection<ResultData> QuestionsWithAnswers { get; set; } 
        
        public string TestName { get => testName; set { testName = value; RaisePropertyChanged(() => TestName); } }
        public int Points { get => points; set { points = value; RaisePropertyChanged(() => Points); } }
        public int CorrectPercent { get => percent; set { percent = value; RaisePropertyChanged(() => CorrectPercent); } }

        #region Command
        public IMvxCommand CloseCommand { get; private set; }
        #endregion
    }
}
