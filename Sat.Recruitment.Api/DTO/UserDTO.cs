using System.ComponentModel.DataAnnotations;

namespace Sat.Recruitment.Api.DTO
{
    public class UserDTO
    {
        [Required(ErrorMessage = "The {0} is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The {0} is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The {0} is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "The {0} is required")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "The {0} is required")]
        public string UserType { get; set; }
        [Required(ErrorMessage = "The {0} is required")]
        public decimal Money { get; set; }
    }
}
