using BDM.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace BDM.Data.UnitScope
{
    
    public interface IUnitScope<TContext> where TContext : DbContext
    {
        TContext Context { get; set; }

        IRepository<T> GetRepository<T>() where T : class;
    
    }
}