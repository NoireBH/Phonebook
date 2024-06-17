using CRUDTEST.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static CRUDTEST.Common.ValidationConstants.PhoneContact;

namespace CRUDTEST.ViewModels
{
    public class PhoneContactDetailsViewModel
    {
        public PhoneContactDetailsViewModel(int id, string firstName, string lastName, string mainPhoneNumber, string emailAddress, int age, byte[] profilePicture)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.MainPhoneNumber = mainPhoneNumber;
            this.EmailAddress = emailAddress;
            this.Age = age;
            this.ProfilePicture = profilePicture;
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
        public string MainPhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public int Age { get; set; }

        public byte[] ProfilePicture { get; set; }

        public List<PhoneNumber> PhoneNumbers { get; set; }
    }
}