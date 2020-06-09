using System.Collections.Generic;
using ViewObj = BDM.Lambda.Model;
using DataObj = BDM.Data.Model;
using BDM.Data.Concrete;
using System.Linq;
using AutoMapper;

namespace BDM.Lambda.Service
{
    public class BrokerService : IBrokerService
    {
        private BDMEntitiesDB _db;
        private IMapper _mapper;

        public BrokerService(BDMEntitiesDB dB, IMapper mapper)
        {
            _db = dB;
            _mapper = mapper;
        }
        public List<ViewObj.Broker> GetBrokerList()
        {
            var brokerlist = _db.Brokers;

            //var brokers = new List<ViewObj.Broker>();            
            // foreach (var data in brokerlist)
            // {
            //     brokers.Add(_mapper.Map<ViewObj.Broker>(data));
            // }
            //return brokers;

            return _mapper.Map<List<ViewObj.Broker>>(brokerlist);
        }
    }
}