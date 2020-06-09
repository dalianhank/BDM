using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BDM.Lambda.Model
{
    public class Email
    {
        public int Id { get; set; }

        public Client Client { get; set; }

        public string ClientName { get; set; }

        public Broker Parent { get; set; }

        public string ParentNPN { get; set; }

        public string EmailAddressType { get; set; }

        public string EmailAddress { get; set; }
    }
}