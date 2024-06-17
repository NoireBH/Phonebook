using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static CRUDTEST.Common.ValidationConstants.PhoneContact;

namespace CRUDTEST.Models
{
    public class PhoneContact
    {
        public PhoneContact(int id, string firstName, string lastName, string PhoneNumber, string emailAddress, int age)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PhoneNumber = PhoneNumber;
            this.EmailAddress = emailAddress;
            this.Age = age;
        }

        public PhoneContact()
        {
                
        }

        [Required]       
        public int Id { get; set; }

        [Required]
        [StringLength(FirstNameMaxLength, ErrorMessage = "First name cannot be more than 50 characters long!")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(LastNameMaxLength, ErrorMessage = "Last name cannot be more than 50 characters long!")]
        public string LastName { get; set; }

        [Required]
        [StringLength(PhoneNumberMaxLength, ErrorMessage = "The phonenumber cannot be more than 10 characters long!")]
        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public int Age { get; set; }

        public byte[] ProfilePicture { get; set; }
    }
}
