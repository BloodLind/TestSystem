using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.DAL.Interfaces;
using TestSystem.DAL.Repositories;
using TestSystem.DAL.TestSystemModel;

namespace TestSystem.BLL.Services
{
    public class InformationService
    {
        private TestSystemRepositoryFactory repositories;
        private List<Discipline> disciplines;
        private List<TestLanguage> languages;
        private List<TestMaterial> testMaterials;
        private List<Theme> themes;
        public InformationService()
        {
            disciplines = new List<Discipline>();
            languages = new List<TestLanguage>();
            testMaterials = new List<TestMaterial>();
            themes = new List<Theme>();
            repositories = new TestSystemRepositoryFactory();
            LoadAllInfoData();
        }
        private void LoadData<T>(List<T> list, IRepository<T> repository) where T : class
        {
            foreach (var data in repository.GetAll())
            {
                list.Add(data);
            }
        }

        private void LoadAllInfoData()
        {
            LoadData<Discipline>(disciplines, repositories.GetDisciplineRepository());
            LoadData<TestLanguage>(languages, repositories.GetTestLanguageRepository());
            LoadData<TestMaterial>(testMaterials, repositories.GetTestMaterialRepository());
            LoadData<Theme>(themes, repositories.GetThemeRepository());
        }

        public List<DTO.Discipline> GetDisciplines()
        {
            List<DTO.Discipline> disciplinList = new List<DTO.Discipline>();

            foreach(var data in disciplines)
            {
                disciplinList.Add(new DTO.Discipline
                {
                    Id = data.Id,
                    Name = data.Name
                });
            }
            return disciplinList;
        }

        public List<DTO.Language> GetLanguages()
        {
            List<DTO.Language> languagList = new List<DTO.Language>();

            foreach(var data in languages)
            {
                languagList.Add(new DTO.Language
                {
                    Id = data.Id,
                    Name = data.Name
                });
            }
            return languagList;  
        }

        public List<DTO.Theme> GetThemes()
        {
            List<DTO.Theme> themeList = new List<DTO.Theme>();
            foreach (var data in themes)
            {
                themeList.Add(new DTO.Theme
                {
                    Id = data.Id,
                    Name = data.Name
                });
            }
            return themeList;
        }
        public List<DTO.TestsMaterial> GetTestsMaterials()
        {
            List<DTO.TestsMaterial> testsList = new List<DTO.TestsMaterial>();
            foreach(var data in testMaterials)
            {
                testsList.Add(new DTO.TestsMaterial
                {
                    Id = data.Id,
                    Name = data.Name,
                    ThemeId = data.ThemeId,
                    Information = data.MaterialsInfo.Information,
                    DisciplineId = data.DisciplineId,
                    LanguageId = data.LanguageId,
                    InformaytionId = data.InformationId
                });
            }
            return testsList;
        }

        public void AddOrUpdateDiscipline(DTO.Discipline discipline)
        {
            var repo = repositories.GetDisciplineRepository();
            Discipline testdiscipline = repo.Get(discipline.Id);
            testdiscipline = testdiscipline == null ? new Discipline() : testdiscipline;
            testdiscipline.Name = discipline.Name;
            repo.CreateOrUpdate(testdiscipline);
        }
        public void AddOrUpdateTheme(DTO.Theme theme)
        {
            var repo = repositories.GetThemeRepository();
            Theme testTheme = repo.Get(theme.Id);
            testTheme = testTheme == null ? new Theme() : testTheme;
            testTheme.Name = theme.Name;
            repo.CreateOrUpdate(testTheme);
        }
        public void AddOrUpdateLanguage(DTO.Language language)
        {
            var repo = repositories.GetTestLanguageRepository();
            TestLanguage testLanguage = repo.Get(language.Id);
            testLanguage = testLanguage == null ? new TestLanguage() : testLanguage;
            testLanguage.Name = language.Name;
            repo.CreateOrUpdate(testLanguage);
        }
        public void AddOrUpdateTestMaterial(DTO.TestsMaterial material)
        {
            var repo = repositories.GetTestMaterialRepository();
            var repoInformation = repositories.GetMaterialInfoRepository();
            MaterialsInfo materialsInfo = repoInformation.Get(material.Id);
            materialsInfo = materialsInfo == null ? new MaterialsInfo() : materialsInfo;
            materialsInfo.Information = material.Information;
            repoInformation.CreateOrUpdate(materialsInfo);

            TestMaterial test = repo.Get(material.Id);
            test = test == null ? new TestMaterial() : test;
            test.InformationId = materialsInfo.Id;
            test.LanguageId = material.LanguageId;
            test.DisciplineId = material.DisciplineId;
            test.ThemeId = material.ThemeId;
            test.Name = material.Name;
            
            repo.CreateOrUpdate(test);
        }
    }
}
