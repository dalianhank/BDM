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
        }
}