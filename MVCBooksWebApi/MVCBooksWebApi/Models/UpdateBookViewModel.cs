namespace MVCBooksWebApi.Models
{
	public class UpdateBookViewModel
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public string ISBN { get; set; }
		public int Pages { get; set; }
		public string Category { get; set; }
		public int Amount { get; set; }
	}
}
