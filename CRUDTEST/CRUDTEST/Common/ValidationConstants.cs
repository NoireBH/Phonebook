using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDTEST.Common
{
    public static class ValidationConstants
    {
        public class PhoneContact
        {
            public const int FirstNameMaxLength = 50;
            public const int LastNameMaxLength = 50;
            public const int PhoneNumberMaxLength = 15;
            public const string PhoneNumberRegex = @"^\+?[0-9]+$";
        }
    }
}