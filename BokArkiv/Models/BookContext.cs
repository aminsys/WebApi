using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace BokArkiv.Models
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {
        }

        public DbSet<Book> BookItems { get; set; }
    }
}
