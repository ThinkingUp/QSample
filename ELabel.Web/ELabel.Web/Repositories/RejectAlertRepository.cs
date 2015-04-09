using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ELabel.Web.DataAccess;

namespace ELabel.Web.Repositories
{
    public class RejectAlertRepository : IRepository<RejectAlert>
    {
        private elabelEntities context;
        private DbSet<RejectAlert> dbSet;

        public RejectAlertRepository()
        {
            context = new elabelEntities();
            dbSet = context.Set<RejectAlert>();
        }

        public void Add(RejectAlert entity)
        {
            dbSet.Add(entity);
        }

        public void AddAll(IEnumerable<RejectAlert> entityList)
        {
            dbSet.AddRange(entityList);
        }

        public void Delete(RejectAlert entity)
        {
            dbSet.Remove(entity);
        }

        public void Put(RejectAlert entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Update(RejectAlert entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateAll(IList<RejectAlert> entity)
        {
            foreach (var item in entity)
            {
                dbSet.Attach(item);
                context.Entry(item).State = EntityState.Modified;
            }
        }

        public IEnumerable<RejectAlert> GetAll()
        {
            return dbSet.ToList();
        }

        public RejectAlert Find(string id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<RejectAlert> FindAll(Expression<Func<RejectAlert, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public int Count(Expression<Func<RejectAlert, bool>> predicate)
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