using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BDM.Data.Enum;

namespace BDM.Data.Model
{
    public class Email
    {
        public int Id { get; set; }

        [Required]
        public Client Client { get; set; }

        public string ClientName { get; set; }

        [Required]
        public Broker Parent { get; set; }

        public string ParentNPN { get; set; }

        public EmailAddressType EmailAddressType { get; set; }

        [Required]
        [Column("Email")]
        public string EmailAddress { get; set; }
    }
}