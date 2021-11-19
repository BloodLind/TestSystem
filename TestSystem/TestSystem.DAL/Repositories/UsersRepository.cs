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
    public class UsersRepository
    {
        private TestSystemDBEntities context;

        
        public UsersRepository(TestSystemDBEntities context)
        {
            this.context = context;
        }
        public int CreateUser(string firstname, string secondname, string fathername)
        {
            int res = context.CreateUserAndGetId(firstname, secondname, fathername);
            context.SaveChanges();
            return res;
        }
        /// <summary>
        /// Return is auntification was created
        /// </summary>
        /// <param name="login">Login of user</param>
        /// <param name="password">Password of user</param>
        /// <returns></returns>
        public bool CreateUserAuntification(string login, string password)
        {
            bool result = context.IsLoginFree(login);
            if(result == true)
                context.CreateUserAuntifaction(login, password, false);
            return result;
        }

        public void SetUserInformation(string login, string password, int userId)
        {
            if (context.CheckAuthorization(login, password))
                context.SetUserInfoForAuntification(login, userId);
        }
        public void EditUserInfo(int userId, string firstname, string secondname, string fatherName)
        {
            context.EditUser(userId, firstname, secondname, fatherName);
        }
        public void SetUserAdmin(string editUserLogin, string login, string password)
        {
            if (context.CheckAuthorization(login, password))
                SetAdminStatement(editUserLogin, true);
        }

        public void RemoveUserAdmin(string editUserLogin, string login, string password)
        {
            if (context.CheckAuthorization(login, password))
                SetAdminStatement(editUserLogin, false);
        }
        public GetUserByLogin_Result Get(string login)
        {
            try
            {
                return context.GetUserByLogin(login).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
        public void SetPhotoForUser(int userId, int? photoId)
        {
            context.SetUserPhotoId(userId, photoId);
        }

        private void SetAdminStatement(string login, bool state)
        {
            context.SetAdminForUser(login, state);
        }
        
        public bool IsAdmin(string login)
        {
            return context.IsAdmin(login);
        }

        public bool CheckAuthorization(string login, string password)
        {
            return context.CheckAuthorization(login, password);
        }
    }
}
