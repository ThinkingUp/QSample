using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ELabel.Web.DataAccess;

namespace ELabel.Web.Repositories
{
    public class DrillholeSamplesViewRespository : IRepository<DrillholeSamplesView>
    {
        private elabelEntities context;
        private DbSet<DrillholeSamplesView> dbSet;

        public DrillholeSamplesViewRespository()
        {
            context = new elabelEntities();
            dbSet = context.Set<DrillholeSamplesView>();
        }

        public void Add(DrillholeSamplesView entity)
        {
            dbSet.Add(entity);
        }

        public void AddAll(IEnumerable<DrillholeSamplesView> entityList)
        {
            dbSet.AddRange(entityList);
        }

        public void Delete(DrillholeSamplesView entity)
        {
            dbSet.Remove(entity);
        }

        public void Put(DrillholeSamplesView entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Update(DrillholeSamplesView entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateAll(IList<DrillholeSamplesView> entity)
        {
            foreach (var item in entity)
            {
                dbSet.Attach(item);
                context.Entry(item).State = EntityState.Modified;
            }
        }

        public IEnumerable<DrillholeSamplesView> GetAll()
        {
            return dbSet.ToList();
        }

        public DrillholeSamplesView Find(string id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<DrillholeSamplesView> FindAll(Expression<Func<DrillholeSamplesView, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public int Count(Expression<Func<DrillholeSamplesView, bool>> predicate)
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