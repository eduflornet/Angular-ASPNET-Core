using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Angular_ASPNET_Core.Models
{
    public class ApiResponse
    {
        public bool Status { get; set; }
        public CustomerModel Customer { get; set; }
        public ModelStateDictionary ModelState { get; set; }
    }
}
