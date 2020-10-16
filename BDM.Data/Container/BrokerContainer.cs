using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BDM.Data.Concrete;
using BDM.Data.Model;
using BDM.Data.Repository;
using BDM.Data.UnitScope;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace BDM.Data.Container{
        public class BrokerContainer : IBrokerContainer
        {
                private readonly IUnitScope<BDMEntitiesDB> _unitScope;
                private readonly IRepository<Broker> _brokerRepository;

                public BrokerContainer(IUnitScope<BDMEntitiesDB> unitScope)
                {
                        _unitScope = unitScope;
                        _brokerRepository = _unitScope.GetRepository<Broker>();
                }
                private readonly Func<IQueryable<Broker>, IIncludableQueryable<Broker, object>> _include =
                                                _ => _.Include(a => a.EmailAddresses);
                

                private readonly Func<IQueryable<Broker>, IOrderedQueryable<Broker>> _orderBy = _ => _.OrderBy(x => x.NPN);  
                public Broker Get(string clientName, string NPN, bool disableTracking = true){
                        var broker = _brokerRepository.Single(predicate: _ => (_.ClientName == clientName) && (_.NPN == NPN),
                                              include: _include, disableTracking: disableTracking);
                        return broker;
                }

                public IList<Broker> GetList(Expression<Func<Broker, bool>> predicate)
                {
                        var Brokers = _brokerRepository.GetList(predicate: predicate, include: _include, orderBy: _orderBy).ToList();                               

                        return Brokers;
                }

                public IList<Broker> GetListByClient(string clientName){
                        return GetList(_ =>_.ClientName == clientName);
                }
        }
}