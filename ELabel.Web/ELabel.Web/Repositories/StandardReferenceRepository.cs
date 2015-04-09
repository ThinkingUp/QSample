using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ELabel.Web.DataAccess;

namespace ELabel.Web.Repositories
{
    public class StandardReferenceRepository : IRepository<standardReference>
    {
        private elabelEntities context;
        private DbSet<standardReference> dbSet;

        public StandardReferenceRepository()
        {
            context = new elabelEntities();
            dbSet = context.Set<standardReference>();
        }

        public void Add(standardReference entity)
        {
            dbSet.Add(entity);
        }

        public void AddAll(IEnumerable<standardReference> entityList)
        {
            dbSet.AddRange(entityList);
        }

        public void Delete(standardReference entity)
        {
            dbSet.Remove(entity);
        }

        public void Put(standardReference entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Update(standardReference entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateAll(IList<standardReference> entity)
        {
            foreach (var item in entity)
            {
                dbSet.Attach(item);
                context.Entry(item).State = EntityState.Modified;
            }
        }

        public IEnumerable<standardReference> GetAll()
        {
            return dbSet.ToList();
        }

        public standardReference Find(string id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<standardReference> FindAll(Expression<Func<standardReference, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public int Count(Expression<Func<standardReference, bool>> predicate)
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