using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ELabel.Web.DataAccess;

namespace ELabel.Web.Repositories
{
    public class LabelRepository : IRepository<Label>
    {
        private elabelEntities context;
        private DbSet<Label> dbSet;

        public LabelRepository()
        {
            context = new elabelEntities();
            dbSet = context.Set<Label>();
        }

        public void Add(Label entity)
        {
            if (entity.LastScanned == null)
                entity.LastScanned = DateTime.Now.ToString();

            if (entity.Status == null)
                entity.Status = "Active";

            if (entity.Location == null)
                entity.Location = "Remote Site";

            if (entity.Scanned == null)
                entity.Scanned = false;

            dbSet.Add(entity);
        }

        public void AddAll(IEnumerable<Label> entityList)
        {
            dbSet.AddRange(entityList);
        }

        public void Delete(Label entity)
        {
            dbSet.Remove(entity);
        }

        public void Put(Label entity)
        {
            if (entity.IsTransferring == true)
            {
                var item = dbSet.Find(entity.SampleID);
                switch (item.Location)
                {
                    case "Remote Site":
                        item.IsTransferring = true;
                        item.Location = "Core Shack";
                        item.Scanned = false;
                        break;
                    case "Core Shack":
                        item.IsTransferring = true;
                        item.Location = "Transport";
                        item.Scanned = false;
                        break;
                    case "Transport":
                        item.IsTransferring = true;
                        item.Location = "Lab";
                        item.Scanned = false;
                        break;
                }
                context.Entry(item).State = EntityState.Modified;
            }
            else
            {
                context.Entry(entity).State = EntityState.Modified;
            }
        }

        public void Update(Label entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateAll(IList<Label> entity)
        {
            foreach (var item in entity)
            {
                dbSet.Attach(item);
                context.Entry(item).State = EntityState.Modified;
            }
        }

        public IEnumerable<Label> GetAll()
        {
            return dbSet.ToList();
        }

        public Label Find(string id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<Label> FindAll(Expression<Func<Label, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public int Count(Expression<Func<Label, bool>> predicate)
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