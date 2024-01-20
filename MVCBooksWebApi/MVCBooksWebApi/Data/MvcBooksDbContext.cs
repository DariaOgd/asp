using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCBooksWebApi.Models.Domain;

namespace MVCBooksWebApi.Data

{
    public class MvcBooksDbContext : IdentityDbContext<IdentityUser>
    {
		public MvcBooksDbContext(DbContextOptions options) : base(options)
		{
		}

        public DbSet<Book> Books { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
