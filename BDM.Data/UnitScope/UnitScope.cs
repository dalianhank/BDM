using System;
using System.Collections.Generic;
using BDM.Data.Concrete;
using BDM.Data.Repository;

namespace BDM.Data.UnitScope
{
        public class UnitScope<TContext> : IUnitScope<TContext> where TContext : BDMEntitiesDB
        {
                private Dictionary<Type, object> _repositories;

                public UnitScope(TContext context)
                {
                        Context = context ?? throw new ArgumentNullException(nameof(context));
                }

                public IRepository<T> GetRepository<T>() where T : class
                {
                        if (_repositories == null) _repositories = new Dictionary<Type, object>();

                        var type = typeof(T);
                        if (!_repositories.ContainsKey(type)) _repositories[type] = new Repository<T>(Context);
                        return (IRepository<T>)_repositories[type];
                }

                public TContext Context { get; set; }
        }
}