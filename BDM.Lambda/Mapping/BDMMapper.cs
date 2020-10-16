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
            CreateMap<DataObj.Broker, ViewObj.Broker>()                
                .ForMember(s =>s.EmailAddresses, c => c.MapFrom(m => m.EmailAddresses));
            CreateMap<DataObj.Email, ViewObj.Email>();
        }
    }
}