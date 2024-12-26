using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GESTIONCOMMANDES.enums;
using Microsoft.AspNetCore.Identity;

namespace GESTIONCOMMANDES.Models.Entities
{
    public class User : IdentityUser
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }

        [Column("createat")]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        [Column("updateat")]
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
        public Role Role { get; set; }
    }
}