using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace FirstMVCApplication.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }


        
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Pin { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
