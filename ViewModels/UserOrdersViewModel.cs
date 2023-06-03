using PRUEBA.Models;

namespace PRUEBA.ViewModels
{
    public class UserOrdersViewModel
    {
        public USERS User { get; set; }
        public List<ORDER> Orders { get; set; }
    }
}