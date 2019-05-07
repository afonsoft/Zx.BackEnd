using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Zx.BackEnd.Data.Interface
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entity);
        IQueryable<TEntity> Get();
        TEntity GetById(long id);
        void Delete(TEntity entity);
        void DeleteById(long id);
        void DeleteRange(Expression<Func<TEntity, bool>> filter);
        void DeleteRange(IEnumerable<TEntity> entity);
        void Update(TEntity entity);
        void UpdateById(TEntity entity, long id);

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
    }
}
