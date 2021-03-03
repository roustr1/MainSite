using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dal.Domain;

namespace Application.Dal
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(string id);
        Task Add(T entity);
        Task Add(IEnumerable<T> entities);
        Task Update(T entity);
        Task Update(IEnumerable<T> entities);
        Task Delete(string id);
        Task Delete(T entity);
        Task Delete(IEnumerable<T> entities);
    }
}