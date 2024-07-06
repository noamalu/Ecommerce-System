using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketBackend.Services.Interfaces
{
    public interface IRepository<T>
{
        public Task<T> GetById(int id);
        Task Add(T entity);
        public Task<IEnumerable<T>> getAll();
        public Task Update(T entity);
        public Task Delete(T entity);
        
}
        

}