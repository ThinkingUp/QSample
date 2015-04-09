using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ELabel.Web.Repositories
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void AddAll(IEnumerable<T> entityList);
        void Delete(T entity);
        void Update(T entity);
        void Put(T entity);
        void UpdateAll(IList<T> entity);
        IEnumerable<T> GetAll();
        T Find(string id);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate);
        int Count(Expression<Func<T, bool>> predicate);
        void SaveChanges();
        void Dispose();
    }
}