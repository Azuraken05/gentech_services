using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace gentech_services.Models
{
    class ProductOrder : INotifyPropertyChanged
    {
        public Customer Customer { get; set; }
        public int SaleID { get; set; }
        public int CustomerID { get; set; }

        public decimal TotalAmount { get; set; }

        public string PaymentMethod { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public User Staff { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
