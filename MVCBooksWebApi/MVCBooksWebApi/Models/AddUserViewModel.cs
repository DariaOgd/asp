namespace MVCBooksWebApi.Models
{
    public class AddUserViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Add the Role property
    }
}