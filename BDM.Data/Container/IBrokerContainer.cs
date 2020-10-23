using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BDM.Data.Model;

namespace BDM.Data.Container{
        public interface IBrokerContainer
        {

                Broker Get(string clientName, string NPN, bool disableTracking = true);
                IList<Broker> GetList(Expression<Func<Broker, bool>> predicate);

                IList<Broker> GetListByClient(string clientName);
                void AddBrokerByClientNPN(string clientName, string NPN, Broker broker);
                void UpdateBrokerByClientNPN(string clientName, string NPN, Broker broker);

                void DeleteBrokerByClientNPN(string clientName, string NPN);
        }
}