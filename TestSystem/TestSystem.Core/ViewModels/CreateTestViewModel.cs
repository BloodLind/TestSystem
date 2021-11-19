using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestSystem.BLL.DTO;
using TestSystem.BLL.Services;

namespace TestSystem.Core.ViewModels
{
    public class CreateTestViewModel : MvxViewModel
    {
        private readonly InformationService informationService;
        private readonly IMvxNavigationService mvxNavigation;
        private readonly TestService testService;
        private Question selectedQuestion;
        private bool isSaving = false;
        private Answer selectedAnswer;
        private string name = "Test";

        public CreateTestViewModel(IMvxNavigationService mvxNavigation, TestService testService, InformationService informationService)
        {
            Questions = new MvxObservableCollection<Question>();
            Answers = new MvxObservableCollection<Answer>();
            this.informationService = informationService;
            this.mvxNavigation = mvxNavigation;
            this.testService = testService;
            InitializateCommands();
            InitializateCollections();
        }

        private void InitializateCollections()
        {
            Languages = new MvxObservableCollection<Language>();
            foreach (var language in informationService.GetLanguages())
                Languages.Add(language);
            SelectedLanguage = Languages[0];

            Disciplines = new MvxObservableCollection<Discipline>();
            foreach (var discipline in informationService.GetDisciplines())
                Disciplines.Add(discipline);
            SelectedDiscipline = Disciplines[0];

            Themes = new MvxObservableCollection<Theme>();
            foreach (var theme in informationService.GetThemes())
                Themes.Add(theme);
            SelectedTheme = Themes[0];


            Answers.Add(new Answer { Data = "Answer" });
            Questions.Add(SelectedQuestion = new Question { QuestionData = "Question" });
            SelectedQuestion.Answers.Add(Answers[0]);
        }
       
        private void InitializateCommands()
        {
            CloseCommand = new MvxCommand(() => mvxNavigation.Close(this));
            MoveNextQuestion = new MvxCommand(() =>
            {
                if(Questions.IndexOf(SelectedQuestion) + 1 < Questions.Count)
                {
                    SelectedQuestion = Questions[Questions.IndexOf(SelectedQuestion) + 1];
                }
                else
                {
                    SelectedQuestion = Questions[0];
                }
                Answers = new MvxObservableCollection<Answer>(SelectedQuestion.Answers);
                RaisePropertyChanged(() => Answers);
            });
            MovePreviousQuestion = new MvxCommand(() =>
            {
                if (Questions.IndexOf(SelectedQuestion) - 1 >= 0)
                {
                    SelectedQuestion = Questions[Questions.IndexOf(SelectedQuestion) - 1];
                }
                else
                {
                    SelectedQuestion = Questions[Questions.Count - 1];
                }
                Answers = new MvxObservableCollection<Answer>(SelectedQuestion.Answers);
                RaisePropertyChanged(() => Answers);
            });
            RemoveQuestion = new MvxCommand(() =>
            {
                if (SelectedQuestion != null)
                    Questions.Remove(SelectedQuestion);
                MovePreviousQuestion.Execute(this); 
            });
            AddQuestionCommand = new MvxCommand(() =>
            {
                SelectedQuestion.CorrectAnswer = SelectedQuestion.CorrectAnswer == null ?
                            SelectedQuestion.Answers[0] : SelectedQuestion.CorrectAnswer;
                SelectedQuestion.Answers = Answers.ToList();
                (Answers = new MvxObservableCollection<Answer>()).Add(new Answer { Data = "Answer" });
                RaisePropertyChanged(() => Answers);
                Questions.Add(SelectedQuestion = new Question { QuestionData = "Question"});
                SelectedQuestion.Answers.Add(Answers[0]);
            });
            AddAnswerCommand = new MvxCommand(() =>
            {
                Answer answer = new Answer { Data = "Answer" };
                SelectedQuestion.Answers.Add(answer);
                Answers.Add(answer);
            });
            RemoveAnswerCommand = new MvxCommand(() =>
            {
                if(Answers.Count != 1)
                    Answers.Remove(SelectedAnswer);
            });
            CreateTestCommand = new MvxCommand(() => Task.Run(() =>
            {
                IsSaving = true;
                SelectedQuestion.CorrectAnswer = SelectedQuestion.CorrectAnswer == null ?
                            SelectedQuestion.Answers[0] : SelectedQuestion.CorrectAnswer;
                Test test = new Test
                {
                    DisciplineId = SelectedDiscipline?.Id,
                    LanguageId = SelectedLanguage?.Id,
                    Questions = Questions.ToList(),
                    ThemeId = SelectedTheme?.Id,
                    Name = this.Name
                };
                testService.AddOrUpdateTest(test);
                AfterTestAdded?.Invoke();
                Thread.Sleep(250);
                IsSaving = false;
                mvxNavigation.Close(this);
            }));
            SelectAsCorrectCommand = new MvxCommand(() => SelectedQuestion.CorrectAnswer = SelectedAnswer);
        }



        public event Action AfterTestAdded;
        public Theme SelectedTheme { get; set; }
        public MvxObservableCollection<Answer> Answers { get; set; }
        public MvxObservableCollection<Question> Questions { get; set; }
        public string Name { get => name; set { name = value; RaisePropertyChanged(() => Name); } }
        public bool IsSaving { get => isSaving; set { isSaving = value; RaisePropertyChanged(() => IsSaving); } }
        public Question SelectedQuestion { get => selectedQuestion; set { selectedQuestion = value; RaisePropertyChanged(() => SelectedQuestion); }}
        public MvxObservableCollection<Discipline> Disciplines { get; set; }
        public MvxObservableCollection<Language> Languages { get; set; }
        public MvxObservableCollection<Theme> Themes { get; set; }
        public Answer SelectedAnswer { get => selectedAnswer; set 
            {
                selectedAnswer = value;
                RaisePropertyChanged(() => SelectedAnswer);
            } }
        public Discipline SelectedDiscipline { get; set; }
        public Language SelectedLanguage { get; set; }


        #region Commands
        public IMvxCommand SelectAsCorrectCommand { get; private set; }
        public IMvxCommand MovePreviousQuestion { get; private set; }
        public IMvxCommand RemoveAnswerCommand { get; private set; }
        public IMvxCommand AddQuestionCommand { get; private set; }
        public IMvxCommand CreateTestCommand { get; private set; }
        public IMvxCommand AddAnswerCommand { get; private set; }
        public IMvxCommand MoveNextQuestion { get; private set; }
        public IMvxCommand RemoveQuestion { get; private set; }
        public IMvxCommand CloseCommand { get; private set; }
        #endregion
    }
}
