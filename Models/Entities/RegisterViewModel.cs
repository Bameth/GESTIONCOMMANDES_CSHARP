using System.ComponentModel.DataAnnotations;
using GESTIONCOMMANDES.enums;

namespace GESTIONCOMMANDES.Models.Entities
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nom is required")]
        public string Nom { get; set; }
        
        [Required(ErrorMessage = "Prenom is required")]
        public string Prenom { get; set; }
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Telephone is required")]
        public string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Adresse is required")]
        public string Adresse { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(40, ErrorMessage = "Password must be at least 8 characters long", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Passwords do not match")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password is required")]
        public string ConfirmPassword { get; set; }
        public Role Role { get; set; } = Role.CLIENT;
    }

}