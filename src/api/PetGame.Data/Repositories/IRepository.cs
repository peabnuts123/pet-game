using System.Linq;
using System.Threading.Tasks;

namespace PetGame.Data
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        IQueryable<TEntity> GetAll();

        // @TODO use real DB
        // Task<TEntity> AddAsync(TEntity entity);
        TEntity Add(TEntity entity);

        // @TODO use real DB
        // Task<TEntity> UpdateAsync(TEntity entity);
        TEntity Update(TEntity entity);

        TEntity Remove(TEntity entity);
    }
}
