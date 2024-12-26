using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GESTIONCOMMANDES.enums;
using Microsoft.AspNetCore.Identity;

namespace GESTIONCOMMANDES.Models.Entities
{
    public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}

}