using gentech_services.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductServicesManagementSystem.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public int ServiceID { get; set; }

        public int StaffID { get; set; }

        public DateTime ScheduleAt { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public Service Service { get; set; }
        public User Staff { get; set; }
        public Customer Customer { get; set; }

    }
}
