using AutoMapper;
using ViewObj = BDM.Lambda.Model;
using DataObj = BDM.Data.Model;
using System.Collections.Generic;

namespace BDM.Lambda.Mapping
{
    public class BDMMapper : Profile
    {
        public BDMMapper()
        {
            CreateMap<DataObj.Broker, ViewObj.Broker>();
            CreateMap<List<DataObj.Broker>, List<ViewObj.Broker>>();
        }
    }
}