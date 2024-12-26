using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GESTIONCOMMANDES.Models.Entities
{
    public abstract class AbstractEntity
    {
        public int Id { get; set; }

        [Column("createat")]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        [Column("updateat")]
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public User? UserCreate { get; set; }

        [NotMapped]
        public User? UserUpdate { get; set; }

        public void OnPrePersist()
        {
            CreateAt = DateTime.UtcNow;
            UpdateAt = DateTime.UtcNow;

            Console.WriteLine($"CreateAt: {CreateAt}, UpdateAt: {UpdateAt}");
        }

        public void OnPreUpdate()
        {
            UpdateAt = DateTime.UtcNow;
        }
    }

}