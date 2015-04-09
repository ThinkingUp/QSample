using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ELabel.Web.DataAccess;

namespace ELabel.Web.Repositories
{
    public class ContaminationCheckRepository : IRepository<ContaminationCheck>
    {
        private elabelEntities context;
        private DbSet<ContaminationCheck> dbSet;

        public ContaminationCheckRepository()
        {
            context = new elabelEntities();
            dbSet = context.Set<ContaminationCheck>();
        }

        public void Add(ContaminationCheck entity)
        {
            dbSet.Add(entity);
        }

        public void AddAll(IEnumerable<ContaminationCheck> entityList)
        {
            dbSet.AddRange(entityList);
        }

        public void Delete(ContaminationCheck entity)
        {
            dbSet.Remove(entity);
        }

        public void Put(ContaminationCheck entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Update(ContaminationCheck entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateAll(IList<ContaminationCheck> entity)
        {
            foreach (var item in entity)
            {
                dbSet.Attach(item);
                context.Entry(item).State = EntityState.Modified;
            }
        }

        public IEnumerable<ContaminationCheck> GetAll()
        {
            return dbSet.ToList();
        }

        public ContaminationCheck Find(string id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<ContaminationCheck> FindAll(Expression<Func<ContaminationCheck, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public int Count(Expression<Func<ContaminationCheck, bool>> predicate)
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