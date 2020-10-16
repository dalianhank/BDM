using BDM.Data.Concrete;
using BDM.Data.Repository;
using BDM.Data.UnitScope;
using Microsoft.Extensions.DependencyInjection;

namespace BDM.Data.Ioc{
        public static class DependenceInjection{
                public static IServiceCollection AddRepository<T>(this IServiceCollection services)
                        where T : class
                {
                        services.AddScoped<IRepository<T>, Repository<T>>();
                        return services;
                }

                public static IServiceCollection AddUnitScope<TContext>(this IServiceCollection services)
                        where TContext : BDMEntitiesDB
                {
                        services.AddScoped<IUnitScope<TContext>, UnitScope<TContext>>();
                        return services;
                }
        }
}