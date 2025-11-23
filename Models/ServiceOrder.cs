using ProductServicesManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace gentech_services.Models
{
    public class ServiceOrder : INotifyPropertyChanged
    {
        private string status;

        public int ServiceOrderID { get; set; }
        public Service Service { get; set; }
        public User Technician { get; set; }
        public DateTime AppointmentDate { get; set; }

        public string Status
        {
            get { return status ?? "Pending"; }
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PaymentMethod { get; set; }
        public Customer Customer { get; set; }
        public int SaleID { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
