using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDTEST.Models
{
    public class PhoneNumber
    {
        public PhoneNumber(string phoneNumber)
        {
            this.Number = phoneNumber;
        }

        public int Id { get; set; }

        public string Number { get; set; }
    }
}