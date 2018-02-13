using System;
namespace FlowersPlanner.Presentation.Domain.Models
{
    public class RegistrationData
    {        
        public string UserName { get; set; } = "Евгений";
        public string PhoneNumber { get; set; } = "+38 (066) 098-22-78";
        public string Email { get; set; } = "zhekin08@gmail.com";
        public int Gender { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
