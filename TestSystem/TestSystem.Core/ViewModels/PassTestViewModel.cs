using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.BLL.DTO;
using TestSystem.Core.Models;

namespace TestSystem.Core.ViewModels
{
    public class PassTestViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService mvxNavigation;
        private readonly Test test;
        private Question question;
        private Answer[] selectedAnswers;
        private int numberOfRows;
        private int numberOfColumns;
        private bool isTestEnding = false;

        public PassTestViewModel(IMvxNavigationService mvxNavigation, Test test)
        {
            this.mvxNavigation = mvxNavigation;
            this.test = test;
            
            Answers = new MvxObservableCollection<Answer>();
            NumberOfColumns = 4;
            InitializateCommands();
            if (test != null && test.Questions?.Count > 0)
            {
                selectedAnswers = new Answer[test.Questions.Count];
                UpdateQuestion(test.Questions.ElementAt(0));
            }
        }

        private void InitializateCommands()
        {
            CloseCommand = new MvxCommand(() => mvxNavigation.Close(this));
            AnswerQuestionCommand = new MvxCommand(() => AnswerQuestion());
            EndTestCommand = new MvxCommand(() => IsEnding = true);
            ReturnToTestCommand = new MvxCommand(() => IsEnding = false);
            ShowResultsCommand = new MvxCommand(() => 
            {
                
                IsEnding = false;
                Task.Run(() => mvxNavigation.Navigate(new TestResultViewModel(mvxNavigation, selectedAnswers, test)));
                mvxNavigation.Close(this);
            }
            );
            MoveNextQuestion = new MvxCommand(() =>
            {
                int index = test.Questions.IndexOf(CurrentQuestion);
                if (test.Questions.Count > index + 1)
                {
                    CurrentQuestion = test.Questions[index + 1];
                    Answers.Clear();
                    foreach (var a in CurrentQuestion.Answers)
                        Answers.Add(a);
                }
            });
            MovePreviousQuestion = new MvxCommand(() =>
            {
                int index = test.Questions.IndexOf(CurrentQuestion);
                if (index - 1 >= 0)
                {
                    CurrentQuestion = test.Questions[index - 1];
                    Answers.Clear();
                    foreach (var a in CurrentQuestion.Answers)
                        Answers.Add(a);
                }
            });
        }

        private void AnswerQuestion()
        {
            if (test.Questions == null)
                return;
            int index = test.Questions.IndexOf(CurrentQuestion);
            selectedAnswers[index] = SelectedAnswer;
            if (test.Questions.Count > index + 1)
            {
                CurrentQuestion = test.Questions[index + 1];
                Answers.Clear();
                foreach (var a in CurrentQuestion.Answers)
                    Answers.Add(a);
            }
            else
                EndTestCommand?.Execute(this);
        }
        private void UpdateQuestion(Question newQuestion)
        {
            CurrentQuestion = newQuestion;
            Answers.Clear();
            foreach(var answer in CurrentQuestion.Answers)
            {
                Answers.Add(answer);
            }
            NumberOfRows = (int)(Answers.Count / NumberOfColumns);

        }
        
        public Question CurrentQuestion { get => question; set { question = value; RaisePropertyChanged(() => CurrentQuestion); } }
        public MvxObservableCollection<Answer> Answers { get; set; }
        public Answer SelectedAnswer { get; set; }
        public int NumberOfRows { get => numberOfRows; set { numberOfRows = value; RaisePropertyChanged(() => NumberOfRows); } }
        public int NumberOfColumns { get => numberOfColumns; set { numberOfColumns = value; RaisePropertyChanged(() => NumberOfRows); } }
        public bool IsEnding { get => isTestEnding; set { isTestEnding = value;RaisePropertyChanged(() => IsEnding); } }

        #region Commands
        public IMvxCommand ReturnToTestCommand { get; private set; }
        public IMvxCommand EndTestCommand { get; private set; }
        public IMvxCommand MovePreviousQuestion { get; private set; }
        public IMvxCommand MoveNextQuestion { get; private set; }
        public IMvxCommand AnswerQuestionCommand { get; private set; }
        public IMvxCommand CloseCommand { get; private set; }
        public IMvxCommand ShowResultsCommand { get; private set; }
        #endregion
    }
}
