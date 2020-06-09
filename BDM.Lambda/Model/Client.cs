using System.Collections.Generic;

namespace BDM.Lambda.Model
{
    public class Client
    {

        public string Name { get; set; }

        public List<Broker> Brokers { get; set; }
    }
}