using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ELabel.Web.DataAccess;

namespace ELabel.Web.Repositories
{
    public class LabAnalysesRepository : IRepository<Laboratory_analysis>
    {
        private elabelEntities context;
        private DbSet<Laboratory_analysis> dbSet;

        public LabAnalysesRepository()
        {
            context = new elabelEntities();
            dbSet = context.Set<Laboratory_analysis>();
        }

        public void Add(Laboratory_analysis entity)
        {
            dbSet.Add(entity);
        }

        public void AddAll(IEnumerable<Laboratory_analysis> entityList)
        {
            dbSet.AddRange(entityList);
        }

        public void Delete(Laboratory_analysis entity)
        {
            dbSet.Remove(entity);
        }

        public void Put(Laboratory_analysis entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Update(Laboratory_analysis entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateAll(IList<Laboratory_analysis> entity)
        {
            foreach (var item in entity)
            {
                dbSet.Attach(item);
                context.Entry(item).State = EntityState.Modified;
            }
        }

        public IEnumerable<Laboratory_analysis> GetAll()
        {
            return dbSet.ToList();
        }

        public Laboratory_analysis Find(string id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<Laboratory_analysis> FindAll(Expression<Func<Laboratory_analysis, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public int Count(Expression<Func<Laboratory_analysis, bool>> predicate)
        {
            return dbSet.Where(predicate).Count();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}