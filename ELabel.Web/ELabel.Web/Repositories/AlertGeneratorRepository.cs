using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ELabel.Web.DataAccess;

namespace ELabel.Web.Repositories
{
    public class AlertGeneratorRepository : IRepository<AlertGenerator>
    {
        private elabelEntities context;
        private DbSet<AlertGenerator> dbSet;

        public AlertGeneratorRepository()
        {
            context = new elabelEntities();
            dbSet = context.Set<AlertGenerator>();
        }

        public void Add(AlertGenerator entity)
        {
            entity.ID = 1;
            if (!dbSet.Any())
                dbSet.Add(entity);
            else
            {
                var item = dbSet.First();
                item.Message = entity.Message;
            }
        }

        public void AddAll(IEnumerable<AlertGenerator> entityList)
        {
            dbSet.AddRange(entityList);
        }

        public void Delete(AlertGenerator entity)
        {
            dbSet.Remove(entity);
        }

        public void Put(AlertGenerator entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Update(AlertGenerator entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateAll(IList<AlertGenerator> entity)
        {
            foreach (var item in entity)
            {
                dbSet.Attach(item);
                context.Entry(item).State = EntityState.Modified;
            }
        }

        public IEnumerable<AlertGenerator> GetAll()
        {
            return dbSet.ToList();
        }

        public AlertGenerator Find(string id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<AlertGenerator> FindAll(Expression<Func<AlertGenerator, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public int Count(Expression<Func<AlertGenerator, bool>> predicate)
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