using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDM.Data.Model
{
    public class Broker
    {
        [Required]
        public Client Client { get; set; }
        [ForeignKey("Client")]
        public string ClientName { get; set; }

        [Required]
        [MaxLength(30)]
        public string NPN { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string SSN { get; set; }
        public List<Email> EmailAddresses { get; set; }
    }
}