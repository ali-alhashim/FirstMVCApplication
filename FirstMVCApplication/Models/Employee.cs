using FirstMVCApplication.Data.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstMVCApplication.Models
{
    [Table("Employee")]
    public class Employee : IdentityUser
    {
       

        public string Name { get; set; }

        public string? Email { get; set; } //from IdentityUser

        public string UserName { get; set; } //from IdentityUser 

        [ForeignKey(nameof(AddressId))]
        public int AddressId { get; set; }
        public ICollection<Address>? Address { get; set; }

      

        public GenderOptions Gender { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:SAR ##,###.##}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "decimal(20, 2)")]
        public decimal Salary { get; set; }
    }
}
