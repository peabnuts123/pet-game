using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PetGame.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly ILogger<Repository<TEntity>> logger;

        public Repository(ILogger<Repository<TEntity>> logger/*, RepositoryPatternDemoContext repositoryPatternDemoContextContext */)
        {
            this.logger = logger;
            this.db = new List<TEntity>();
        }

        private IList<TEntity> db;

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                // @TODO use a real DB
                // return _repositoryPatternDemoContextContext.Set<TEntity>();
                return this.db.AsQueryable();
            }
            catch (Exception)
            {
                throw new Exception("Couldn't retrieve entities");
            }
        }

        public TEntity Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                // @TODO use a real DB
                // await _repositoryPatternDemoContextContext.AddAsync(entity);
                // await _repositoryPatternDemoContextContext.SaveChangesAsync();
                this.db.Add(entity);

                return entity;
            }
            catch (Exception)
            {
                throw new Exception($"{nameof(entity)} could not be saved");
            }
        }

        public TEntity Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            int existingEntityIndex = this.db.IndexOf(entity);

            if (existingEntityIndex == -1)
            {
                throw new ArgumentException($"Cannot update entity, it is not present in the database");
            }

            try
            {
                // @TODO use real DB
                // _repositoryPatternDemoContextContext.Update(entity);
                // await _repositoryPatternDemoContextContext.SaveChangesAsync();
                this.db[existingEntityIndex] = entity;

                return entity;
            }
            catch (Exception)
            {
                throw new Exception($"{nameof(entity)} could not be updated");
            }
        }

        public TEntity Remove(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            int existingEntityIndex = this.db.IndexOf(entity);

            if (existingEntityIndex == -1)
            {
                throw new ArgumentException($"Cannot update entity, it is not present in the database");
            }

            try
            {
                this.db.RemoveAt(existingEntityIndex);

                return entity;
            }
            catch (Exception)
            {
                throw new Exception($"{nameof(entity)} could not be updated");
            }
        }

    }
}