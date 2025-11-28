using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gentech_services.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; }

        [Required, MaxLength(255)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required, MaxLength(4)]
        public string PIN { get; set; }

        [MaxLength(50)]
        public string Role { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation
        /*
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Sale> Sales { get; set; }
        public ICollection<InventoryLog> InventoryLogs { get; set; }
        */
    }
}
