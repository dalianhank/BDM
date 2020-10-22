using System.Collections.Generic;
using ViewObj = BDM.Lambda.Model;
using DataObj = BDM.Data.Model;
using BDM.Data.Concrete;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BDM.Data.Container;

namespace BDM.Lambda.Service
{
        public class BrokerService : IBrokerService
    {
        private BDMEntitiesDB _db;   
        private IBrokerContainer _brokerContainer;     
        private IMapper _mapper;


        public BrokerService(BDMEntitiesDB dB, IMapper mapper, IBrokerContainer brokerContainer)
        {
            _db = dB;            
            _mapper = mapper;
            _brokerContainer = brokerContainer;
        }

        
        public List<ViewObj.Broker> GetBrokerList(string clientName)
        {
            List<DataObj.Broker> brokerlist = _db.Brokers
                                                .Where(b =>b.ClientName == clientName)
                                                .Include(e => e.EmailAddresses)
                                                .ToList();

            var brokers = _brokerContainer.GetListByClient(clientName);

            return _mapper.Map<List<ViewObj.Broker>>(brokers);
            
        }

        public ViewObj.Broker GetBrokerByClientNPN(string clientName, string npn){
            var broker = _brokerContainer.Get(clientName, npn);
            return _mapper.Map<ViewObj.Broker>(broker);
        }
    }
}