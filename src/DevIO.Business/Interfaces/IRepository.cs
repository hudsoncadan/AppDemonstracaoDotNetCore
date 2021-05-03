using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IRepository<TEntity> : IDisposable 
        where TEntity : Entity
    {
        Task Add(TEntity entity);

        Task<TEntity> GetById(Guid id);
        
        Task<IEnumerable<TEntity>> GetAll();
        
        Task Update(TEntity entity);
        
        Task Delete(Guid id);

        Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate);

        Task<int> SaveChanges();
    }
}
