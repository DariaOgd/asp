using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCBooksWebApi.Data;
using MVCBooksWebApi.Models;
using MVCBooksWebApi.Models.Domain;



namespace MVCBooksWebApi.Controllers
{

    [Authorize]
    public class BooksController : Controller
	{
		private readonly MvcBooksDbContext mvcBooksDbContext;

		public BooksController(MvcBooksDbContext mvcBooksDbContext)
		{
			this.mvcBooksDbContext = mvcBooksDbContext;
		}


        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm)
        {
            // Get all books by default or filter based on search term
            IQueryable<Book> query = mvcBooksDbContext.Books;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Apply search filter if search term is provided
                searchTerm = searchTerm.ToLower();
                query = query.Where(b =>
                    b.Title.ToLower().Contains(searchTerm) ||
                    b.Author.ToLower().Contains(searchTerm) ||
                    b.ISBN.ToLower().Contains(searchTerm) ||
                    b.Category.ToLower().Contains(searchTerm));
            }

            var books = await query.ToListAsync();
            return View(books);
        }




        public IActionResult Add()
		{
			return View();
		}

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddBookViewModel addBookRequest)
        {
            var existingBookWithISBN = await mvcBooksDbContext.Books
                .FirstOrDefaultAsync(b => b.ISBN == addBookRequest.ISBN);

            if (existingBookWithISBN != null)
            {
                // Book with the same ISBN already exists
                TempData["ErrorMessage"] = "ISBN już istnieje w bazie";
                return View("Add", addBookRequest);
            }

            var book = new Book()
            {
                Id = Guid.NewGuid(),
                Title = addBookRequest.Title,
                Author = addBookRequest.Author,
                ISBN = addBookRequest.ISBN,
                Pages = addBookRequest.Pages,
                Category = addBookRequest.Category,
                Amount = addBookRequest.Amount
            };

            await mvcBooksDbContext.Books.AddAsync(book);
            await mvcBooksDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpGet]
		public async Task<IActionResult> View(Guid id)
		{
			var book = await mvcBooksDbContext.Books.FirstOrDefaultAsync(x => x.Id == id);

			if (book != null)
			{ 

				var viewModel = new UpdateBookViewModel()
				{
					Id = book.Id,
					Title = book.Title,
					Author = book.Author,
					ISBN = book.ISBN,
					Pages = book.Pages,
					Category = book.Category,
					Amount = book.Amount
				};
                return await Task.Run(() => View("View", viewModel));
            }
			
            return RedirectToAction("Index");
        }
		[HttpPost]
		[HttpPost]
public async Task<IActionResult> View(UpdateBookViewModel model)
{
    var book = await mvcBooksDbContext.Books.FindAsync(model.Id);

    if (book != null)
    {
        book.Title = model.Title;
        book.Author = model.Author;
        book.ISBN = model.ISBN;
        book.Pages = model.Pages;
        book.Category = model.Category;
        book.Amount = model.Amount;

        await mvcBooksDbContext.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    return RedirectToAction("Index");
}


		[HttpPost]
		public async Task<IActionResult> Delete(UpdateBookViewModel model)
		{
			var book = await mvcBooksDbContext.Books.FindAsync(model.Id);

			if (book != null)
			{
				mvcBooksDbContext.Books.Remove(book);
				await mvcBooksDbContext.SaveChangesAsync();

				return RedirectToAction("Index");
			}
            return RedirectToAction("Index");
        }

	}
}
