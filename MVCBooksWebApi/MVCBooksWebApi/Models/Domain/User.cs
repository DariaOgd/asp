﻿
namespace MVCBooksWebApi.Models.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Add a property for the user's role



    }

}