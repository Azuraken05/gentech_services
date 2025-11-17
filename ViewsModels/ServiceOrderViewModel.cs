using gentech_services.MVVM;
using ProductServicesManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace gentech_services.ViewsModels
{
    internal class ServiceOrderViewModel : ViewModelBase
    {
        private Appointment currentAppointment;

        public Appointment CurrentAppointment
        {
            get { return currentAppointment; }
            set { currentAppointment = value; OnPropertyChanged(); }
        }
    }
}
