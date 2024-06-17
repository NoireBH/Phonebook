using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static CRUDTEST.Common.ValidationConstants.PhoneContact;

namespace CRUDTEST.ViewModels
{
    public class PhoneContactViewModel
    {
        public PhoneContactViewModel(int id, string firstName, string lastName, string PhoneNumber)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PhoneNumber = PhoneNumber;
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
    }
}