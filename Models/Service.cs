using gentech_services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductServicesManagementSystem.Models
{
    public class Service
    {
        public int ServiceID { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        //TODO: figure out what to do with this
        public int CategoryID { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Category Category { get; set; }
        //public ICollection<Appointment> Appointments { get; set; }
    }
}
