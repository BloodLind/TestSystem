using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.DAL.Interfaces;
using TestSystem.DAL.TestSystemModel;

namespace TestSystem.DAL.Repositories
{
    public sealed class TestSystemRepositoryFactory
    {
        private TestSystemDBEntities context = new TestSystemDBEntities();

        public IRepository<Answer> GetAnswerRepository()
            => new Repository<Answer>(context);

        public IRepository<Question> GetQuestionrRepository()
             => new Repository<Question>(context);

        public IRepository<QuestionAnswer> GetQuestionAnswerRepository()
            => new Repository<QuestionAnswer>(context);

        public IRepository<Photo> GetPhotoRepository()
           => new Repository<Photo>(context);

        public IRepository<Test> GetTestRepository()
          => new Repository<Test>(context);

        public IRepository<Result> GetResultRepository()
          => new Repository<Result>(context);

        public IRepository<TestMaterial> GetTestMaterialRepository()
          => new Repository<TestMaterial>(context);

        public IRepository<TestLanguage> GetTestLanguageRepository()
          => new Repository<TestLanguage>(context);

        public IRepository<Theme> GetThemeRepository()
          => new Repository<Theme>(context);

        public IRepository<MaterialsInfo> GetMaterialInfoRepository()
          => new Repository<MaterialsInfo>(context);

        public IRepository<Discipline> GetDisciplineRepository()
          => new Repository<Discipline>(context);

        public UsersRepository GetUserRepository()
          => new UsersRepository(context);
    }
}
