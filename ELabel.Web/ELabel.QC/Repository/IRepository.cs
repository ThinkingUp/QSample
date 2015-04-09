using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ELabel.QC.Repository
{
    public interface IRepository<T>
    {
        long Add(T entity);
        bool AddAll(IList<T> entityList);
        void Delete(T entity);
        void Update(T entity);
        void UpdateAll(IList<T> entity);
        T GetById(long id);
        IList<T> FindAll();
        IList<T> FindAll(Expression<Func<T, bool>> predicate);
        int Count(Expression<Func<T, bool>> predicate);
        void RefreshSession();

    }
}
