using System.Collections.Generic;
using System.Threading.Tasks;

namespace KiancaAPI.Repository
{
    public interface IRepository<T>
    {
        // api/[GET]
        Task<IEnumerable<T>> Get();
        // api/1/[GET]
        Task<T> Get(string id);
        // api/[POST]
        Task Create(T entity);
        // api/[PUT]
        Task<bool> Update(T entity);
        // api/1/[DELETE]
        Task<bool> Delete(string id);
    }
}
