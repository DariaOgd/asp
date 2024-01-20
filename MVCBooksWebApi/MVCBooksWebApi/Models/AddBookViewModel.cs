namespace MVCBooksWebApi.Models
{
    public class AddBookViewModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int Pages { get; set; }
        public string Category { get; set; }
        public int Amount { get; set; }
    }
}
