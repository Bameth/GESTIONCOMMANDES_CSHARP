using System.ComponentModel.DataAnnotations;

namespace GESTIONCOMMANDES.Models.Entities
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(40, ErrorMessage = "Password must be at least 8 characters long", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Compare("ConfirmNewPassword", ErrorMessage = "Passwords do not match")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password is required")]
        [Display(Name = "Confirm New Password")]
        public string ConfirmNewPassword { get; set; }
    }

}