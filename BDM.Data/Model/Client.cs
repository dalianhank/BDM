using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BDM.Data.Model
{
    public class Client
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public List<Broker> Brokers { get; set; }
    }
}