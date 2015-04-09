using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ELabel.Web.DataAccess;

namespace ELabel.Web.Repositories
{
    public class DrillholesPlanningRespository : IRepository<DrillholesPlanning>
    {
        private elabelEntities context;
        private DbSet<DrillholesPlanning> dbSet;

        public DrillholesPlanningRespository()
        {
            context = new elabelEntities();
            dbSet = context.Set<DrillholesPlanning>();
        }

        public void Add(DrillholesPlanning entity)
        {
            dbSet.Add(entity);
        }

        public void AddAll(IEnumerable<DrillholesPlanning> entityList)
        {
            dbSet.AddRange(entityList);
        }

        public void Delete(DrillholesPlanning entity)
        {
            dbSet.Remove(entity);
        }

        public void Put(DrillholesPlanning entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Update(DrillholesPlanning entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateAll(IList<DrillholesPlanning> entity)
        {
            foreach (var item in entity)
            {
                dbSet.Attach(item);
                context.Entry(item).State = EntityState.Modified;
            }
        }

        public IEnumerable<DrillholesPlanning> GetAll()
        {
            return dbSet.ToList();
        }

        public DrillholesPlanning Find(string id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<DrillholesPlanning> FindAll(Expression<Func<DrillholesPlanning, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public int Count(Expression<Func<DrillholesPlanning, bool>> predicate)
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