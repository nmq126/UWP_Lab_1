using System;
using System.Text.RegularExpressions;

namespace UWP_Assignment.Utils
{
    public class Validator
    {
        private static readonly string EmailPattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        private static readonly string PhonePattern = @"^(84|0[3|5|7|8|9])+([0-9]{8})$";
        private static readonly string SongLinkPattern = @"^(https?|ftp|file):\/\/(www.)?(.*?)\.(mp3)$";
        public static bool IsEmail(string email)
        {
            return Regex.IsMatch(email, EmailPattern);
        }

        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, PhonePattern).Success;

        }
        public static bool IsValidSongLink(string link)
        {
            return Regex.IsMatch(link, SongLinkPattern);
        }
        public static bool IsEmpty(string text)
        {
            return string.IsNullOrEmpty(text);
        }
        public static bool MatchString(string string1, string string2)
        {
            return string.Equals(string1, string2);
        }
    }
}
