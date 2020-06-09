using System.Collections.Generic;
using BDM.Lambda.Model;

namespace BDM.Lambda.Service
{
    public interface IBrokerService
    {
        List<Broker> GetBrokerList();
    }
}