using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketBackend.Services.Interfaces
{
    public interface IRepository<T>
{
        T GetById(int id);
        void Add(T entity);
        IEnumerable<T> getAll();
        void Update(T entity);
        void Delete(T entity);
}
        

}