using FirstMVCApplication.Data.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstMVCApplication.Models
{
    [Table("Employee")]
    public class Employee : IdentityUser
    {
        [Key]
        public int EmployeeId { get; set; }

        public string Name { get; set; }

        public string? Email { get; set; }

        //public int AddressId { get; set; }
        public ICollection<Address>? Address { get; set; }

      

        public GenderOptions Gender { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:SAR ##,###.##}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "decimal(20, 2)")]
        public decimal Salary { get; set; }
    }
}
