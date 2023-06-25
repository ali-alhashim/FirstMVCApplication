using FirstMVCApplication.Models;

namespace FirstMVCApplication.ViewModels
{
    public class ViewModelEmployeeAddress
    {
        public Employee Employee { get; set; }
        public List<Address>? Address { get; set; }
    }
}
