using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab_1.Utils
{
    class Validator
    {
        private static readonly string EmailPattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        private static readonly string PhonePattern = @"^(84|0[3|5|7|8|9])+([0-9]{8})$";
        public static bool IsEmail(string email)
        {
            return Regex.IsMatch(email, EmailPattern);
        }

        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, PhonePattern).Success;

        }
        public static bool IsEmpty(string text)
        {
            return String.IsNullOrEmpty(text);
        }
        public static bool MatchString(string string1, string string2)
        {
            return String.Equals(string1, string2);
        }
    }
}
