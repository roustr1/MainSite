using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Dal.Domain;

namespace Application.Dal
{
    public interface IRepository<T> where T : BaseEntity
    {
        void Add(T entity);
        void Add(IEnumerable<T> entities);
        void Update(T entity);
        void Update(IEnumerable<T> entities);
        void Delete(string id);
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
        T Get(string id);
        T Get(Expression<Func<T, Boolean>> where);
        IEnumerable<T> GetMany(Expression<Func<T, Boolean>> where);
        IEnumerable<T> GetAll();
    }
}