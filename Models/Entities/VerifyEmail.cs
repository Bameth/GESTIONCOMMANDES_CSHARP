using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GESTIONCOMMANDES.enums;
using Microsoft.AspNetCore.Identity;

namespace GESTIONCOMMANDES.Models.Entities
{
    public class VerifyEmail
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
    }

}