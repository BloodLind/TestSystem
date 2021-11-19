using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.DAL.Repositories;
using TestSystem.DAL.TestSystemModel;
using TestSystem.BLL;
using TestSystem.DAL.Interfaces;

namespace TestSystem.BLL.Services
{
    public class TestService
    {
        private TestSystemRepositoryFactory repositories = new TestSystemRepositoryFactory();
        
        private List<Question> questions;
        private List<Test> tests;
        private List<QuestionAnswer> testsContext;
        private List<Result> results;
        private List<Answer> answers;

        public TestService()
        {
            questions = new List<Question>();
            tests = new List<Test>();
            testsContext = new List<QuestionAnswer>();
            results = new List<Result>();
            answers = new List<Answer>();

            LoadAllTestData();
        }

        private void LoadData<T>(List<T> list, IRepository<T> repository) where T: class
        {
            foreach(var data in repository.GetAll())
            {
                list.Add(data);
            }
        }
        

        private void LoadAllTestData()
        {
            LoadData<Question>(questions, repositories.GetQuestionrRepository());
            LoadData<QuestionAnswer>(testsContext, repositories.GetQuestionAnswerRepository());
            LoadData<Result>(results, repositories.GetResultRepository());
            LoadData<Test>(tests, repositories.GetTestRepository());
            LoadData<Answer>(answers, repositories.GetAnswerRepository());
        }

        public List<DTO.Test> GetTests()
        {
            List<DTO.Test> testList = new List<DTO.Test>();
            foreach (var test in tests)
            {
                testList.Add(new DTO.Test 
                { 
                    Id = test.Id,
                    Name = test.Name,
                    LanguageId = test.LanguageId,
                    DisciplineId = test.DisciplineId,
                    ThemeId = test.ThemeId
                });

                
                foreach (var context in test.QuestionAnswers
                                              .GroupBy(p => p.QuestionId)
                                              .Select(g => g.First())
                                              .ToList())
                {
                    testList[testList.Count - 1].Questions.Add(new DTO.Question
                    {
                        Id = context.QuestionId,
                        DisciplineId = context.Question.DisciplineId,
                        ThemeId = context.Question.ThemeId,
                        QuestionData = context.Question.QuestionData
                    });
                }
            }

            foreach(var test in testList)
            {
                foreach(var answer in testsContext)
                {
                    DTO.Question question = test.Questions.Find((x) => x.Id == answer.QuestionId);
                    if (question == null)
                        continue;
                    question?
                        .Answers.Add(new DTO.Answer 
                        { 
                            Id = answer.Answer.Id,
                            DisciplineId = answer.Answer.DisciplineId,
                            ThemeId =answer.Answer.ThemeId,
                            Data = answer.Answer.AnswerData
                            
                        });

                    if (answer.IsAnswerCorrect)
                        question.CorrectAnswer = question.Answers.Last();
                }
            }

            return testList;
        }
        public List<DTO.Question> GetQuestions()
        {
            List<DTO.Question> questionList = new List<DTO.Question>();

            foreach(var data in questions)
            {
                questionList.Add(new DTO.Question
                {
                    Id = data.Id,
                    QuestionData = data.QuestionData,
                    DisciplineId = data.DisciplineId,
                    ThemeId = data.ThemeId
                });
            }    
                    
            return questionList;
        }
        public void DeleteTest(int id)
        {
            repositories.GetTestRepository().Delete(id);
        }
        public List<DTO.Answer> GetAnswers()
        {
            List<DTO.Answer> answersList = new List<DTO.Answer>();

            foreach(var data in answers)
            {
                answersList.Add(new DTO.Answer 
                {
                    Id = data.Id,
                    Data = data.AnswerData,
                    DisciplineId = data.DisciplineId,
                    ThemeId = data.ThemeId
                });
            }
            return answersList;
        }
        public List<DTO.Result> GetResults()
        {
            List<DTO.Result> resultsList = new List<DTO.Result>();
            
            foreach(var data in results)
            {
                resultsList.Add(new DTO.Result 
                {
                    Id = data.Id,
                    CorrectPercent = data.CorrectPercent,
                    Points = data.Points,
                    TestId = Convert.ToInt32(data.TestId),
                    UserId = data.UserId
                });
            }
            return resultsList;
        }

        
        public void AddOrUpdateTest(DTO.Test test)
        {
            var testr = repositories.GetTestRepository();
            Test dalTest = testr.Get(test.Id);
            dalTest = dalTest == null ? new Test() : dalTest;
            dalTest.DisciplineId = test.DisciplineId;
            dalTest.LanguageId = test.LanguageId;
            dalTest.ThemeId = test.ThemeId;
            dalTest.Name = test.Name;
            testr.CreateOrUpdate(dalTest);
            

            var testqa = repositories.GetQuestionAnswerRepository();
            var questionr = repositories.GetQuestionrRepository();
            var answerr = repositories.GetAnswerRepository();

            foreach(var data in test.Questions)
            {
                Question dalQuestion = questionr.Get(data.Id);
                dalQuestion = dalQuestion == null ? new Question() : dalQuestion;
                dalQuestion.QuestionData = data.QuestionData;
                dalQuestion.ThemeId = data.ThemeId;
                dalQuestion.DisciplineId = data.DisciplineId;

                questionr.CreateOrUpdate(dalQuestion);
                foreach (var answer in data.Answers)
                {
                    Answer dalAnswer = answerr.Get(answer.Id);
                    dalAnswer = dalAnswer == null ? new Answer() : dalAnswer;
                    dalAnswer.AnswerData = answer.Data;
                    dalAnswer.DisciplineId = answer.DisciplineId;
                    dalAnswer.ThemeId = answer.ThemeId;
                    answerr.CreateOrUpdate(dalAnswer);

                    testqa.CreateOrUpdate(new QuestionAnswer
                    {
                        Testid = dalTest.Id,
                        QuestionId = dalQuestion.Id,
                        AnswerId = dalAnswer.Id,
                        IsAnswerCorrect = answer == data.CorrectAnswer
                    });
                }
            }

        }

        public void AddResult(DTO.Result result)
        {
            repositories.GetResultRepository().CreateOrUpdate(new Result
            {
                CorrectPercent = result.CorrectPercent,
                Points = result.Points,
                TestId = result.TestId,
                UserId = result.UserId
            });
        }
    }
}
