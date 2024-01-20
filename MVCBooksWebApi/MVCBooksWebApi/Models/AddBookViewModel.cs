using System.ComponentModel.DataAnnotations;

namespace MVCBooksWebApi.Models
{
    public class AddBookViewModel
    {
        [Required(ErrorMessage = "Pole Tytuł jest wymagane.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Pole Autor jest wymagane.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Pole ISBN jest wymagane.")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Pole Ilość stron jest wymagane.")]
        public int Pages { get; set; }

        [Required(ErrorMessage = "Pole Kategoria jest wymagane.")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Pole Ilość jest wymagane.")]
        public int Amount { get; set; }
    }

}
