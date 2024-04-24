using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task Add(T item);
        void Delete(T item);
        void Update(T item);
        Task<T> GetId(int id);
        Task<IEnumerable<T>> GetAll();
    }
}
