using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dal.Domain;

namespace Application.Dal
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T Get(string id);
        void Add(T entity);
        void Add(IEnumerable<T> entities);
        void Update(T entity);
        void Update(IEnumerable<T> entities);
        void Delete(string id);
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
    }
}