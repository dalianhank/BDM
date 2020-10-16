using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace BDM.Data.Repository{

        
        public interface IRepository<T> where T: class{

                T Single(Expression<Func<T, bool>> predicate = null,
                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                        bool disableTracking = true);

                IQueryable<T> Get(Expression<Func<T, bool>> filter = null,
                                                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                string includeProperties = "");

               IQueryable<T> GetList(Expression<Func<T, bool>> predicate = null,
                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                        bool disableTracking = true); 
        }
}