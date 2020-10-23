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

                public void AddBrokerByClientNPN(string clientName, string NPN, Broker broker)
                {
                        broker.ClientName = clientName;
                        broker.NPN = NPN;
                        _brokerRepository.Add(broker);
                        _unitScope.SaveChanges();
                }

                public void UpdateBrokerByClientNPN(string clientName, string NPN, Broker broker)
                {
                        var dbBroker = Get(clientName, NPN, false);
                        if(dbBroker != null){
                                MergeBroker(broker, dbBroker);
                                _unitScope.SaveChanges();
                        }
                        else{
                                AddBrokerByClientNPN(clientName,NPN, broker);
                        }
                }

                public void DeleteBrokerByClientNPN(string clientName, string NPN)
                {
                        var dbBroker = Get(clientName, NPN, false);
                        if(dbBroker != null){
                                _brokerRepository.Delete(dbBroker);
                                _unitScope.SaveChanges();
                        }
                        
                }
                
                private static void MergeBroker(Broker source, Broker target){
                        target.FirstName = source.FirstName;
                        target.LastName = source.LastName;
                        target.MiddleName = source.MiddleName;
                        target.Suffix = source.Suffix;
                        target.DateOfBirth = source.DateOfBirth;
                        target.SSN = source.SSN;
                        if(target.EmailAddresses == null) target.EmailAddresses = new List<Email>();
                        MergeEmails(source.EmailAddresses, target.EmailAddresses);
                }

                private static void MergeEmails(List<Email> source, List<Email> target)
                {
                        foreach (var sourceItem in source)
                        {
                                var targetItem = target.FirstOrDefault(_ => _.Id == sourceItem.Id ||
                                                                                _.EmailAddressType == sourceItem.EmailAddressType);

                                if (targetItem != null)
                                {
                                        targetItem.EmailAddress = sourceItem.EmailAddress;
                                        targetItem.EmailAddressType = sourceItem.EmailAddressType;
                                }
                                else
                                {
                                        target.Add(sourceItem);
                                }
                        }
                }
        }
}