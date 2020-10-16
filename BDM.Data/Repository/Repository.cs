using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BDM.Data.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace BDM.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        internal BDMEntitiesDB _db;
        internal DbSet<T> _dbSet;

        public Repository(BDMEntitiesDB db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }


        public T Single(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return orderBy(query).FirstOrDefault();
            return query.FirstOrDefault();
        }
        
        public IQueryable<T> Get(Expression<Func<T, bool>> filter = null,
                                                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public IQueryable<T> GetList(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
                    Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true)
                {
                    IQueryable<T> query = _dbSet;
                    if (disableTracking)
                    {
                        query = query.AsNoTracking();
                    }

                    if (include != null)
                    {
                        query = include(query);
                    }

                    if (predicate != null)
                    {
                        query = query.Where(predicate);
                    }

                    return orderBy != null ? orderBy(query) : query;
                }

        public T GetByID(object id)
        {
            return _dbSet.Find(id);
        }

        public void Insert(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(object id)
        {
            T entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(T entityToDelete)
        {
            _dbSet.Remove(entityToDelete);
        }

        public void Update(T entityToUpdate)
        {
            _dbSet.Update(entityToUpdate);
        }
    }
}