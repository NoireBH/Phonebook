using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDTEST.Models
{
    public class PhoneNumber
    {
        public PhoneNumber(int id, string phoneNumber)
        {
            this.Number = phoneNumber;
            this.Id = id;
        }

        public PhoneNumber(string phoneNumber)
        {
            this.Number = phoneNumber;
        }

        public int Id { get; set; }

        public string Number { get; set; }
    }
}