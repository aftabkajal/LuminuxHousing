using AppCore.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Entities
{
   public class ContactUs : Entity
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Email { get; set; }
    }
}
