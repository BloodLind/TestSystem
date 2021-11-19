using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using TestSystem.BLL.DTO;
using TestSystem.DAL.Repositories;
using TestSystem.DAL.TestSystemModel;

namespace TestSystem.BLL.Services
{
    public sealed class UserService
    {
        private UsersRepository userRepository;
        private TestSystemDBEntities context = new TestSystemDBEntities();
        public UserService()
        {
            userRepository = new UsersRepository(context);
        }

        public User GetUser(string login)
        {
            
            GetUserByLogin_Result getUserByLogin_Result = userRepository.Get(login);

            return new User
            {
                Firstname = getUserByLogin_Result.Firstname,
                Fathername = getUserByLogin_Result.Fathername,
                Secondname = getUserByLogin_Result.Secondname,
                Id = getUserByLogin_Result.Id == null ? -1 : (int)getUserByLogin_Result.Id,
                Photo = getUserByLogin_Result.ProfilePicture
                };
            
            
        }
        public bool CreateUser(string login, string password, User user)
        {
            if (userRepository.CreateUserAuntification(login, password))
            {
                user.Id = userRepository.CreateUser(user.Firstname, user.Secondname, user.Fathername);
                userRepository.SetUserInformation(login, password, user.Id);
                return true;
            }
            else
                return false;
        }

        public void UpdateInformation(User user)
        {
            userRepository.EditUserInfo(user.Id, user.Firstname, user.Secondname, user.Fathername);
        }
        public void UpdatePhoto(User user)
        {
            TestSystemRepositoryFactory testSystemRepositoryFactory = new TestSystemRepositoryFactory();
            var repository = testSystemRepositoryFactory.GetPhotoRepository();
            repository.CreateOrUpdate(new Photo { Picture = user.Photo });
            int id = repository.GetAll().Last().Id;
            userRepository.SetPhotoForUser(user.Id, id);
        }
        public void SetAdminState(string login, string password, string loginEdit, bool state)
        {
            if (state)
                userRepository.SetUserAdmin(loginEdit, login, password);
            else
                userRepository.RemoveUserAdmin(loginEdit, login, password);
        }

        public bool IsAdmin(string login)
        {
            return userRepository.IsAdmin(login);
        }
       
       public bool CheckAuthorization(string login, string password)
        {
            return userRepository.CheckAuthorization(login, password);
        }
    }
}
