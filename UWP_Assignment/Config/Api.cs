using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Assignment.Config
{
    public class Api
    {
        public static string mediaType = "applicaion/json";
        public static string apiDomain = "https://music-i-like.herokuapp.com";
        public static string accountPath = "/api/v1/accounts";
        public static string loginPath = "/api/v1/accounts/authentication";
        public static string songPath = "/api/v1/songs";
        public static string mySongPath = "/api/v1/songs/mine";
    }
}
