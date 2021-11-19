using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.DAL.Interfaces;

namespace TestSystem.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DbContext database;
        private DbSet<T> table;

        public Repository(DbContext dbContext)
        {
            database = dbContext;
            table = database.Set<T>();
        }

        public void CreateOrUpdate(T entity)
        {

            table.AddOrUpdate(entity);
            SaveChanges();
        }

        public void Delete(T entity)
        {
            table.Remove(entity);
            SaveChanges();
        }

        public void Delete(int id)
        {
            table.Remove(Get(id));
            SaveChanges();
        }

        public T Get(int id)
        {

            return table.Find(id);
        }

        public IEnumerable<T> GetAll()
        {

            return table;
        }

        public void SaveChanges()
        {
            try
            {
                database.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                {
                    File.AppendAllText("log.txt", "Object: " + validationError.Entry.Entity.ToString());
                    File.AppendAllText("log.txt", " ");
                    foreach (DbValidationError err in validationError.ValidationErrors)
                    {
                        File.AppendAllText("log.txt", err.ErrorMessage + "\n");
                    }
                }
            }
        }
    }
}
