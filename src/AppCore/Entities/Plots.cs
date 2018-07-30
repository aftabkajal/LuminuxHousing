using AppCore.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Entities
{
   public class Plots : Entity
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public DateTime ConstructionStartDate { get; set; }
        public string Note { get; set; }
        public bool IsSold { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhoneNumber { get; set; }
    }
}
