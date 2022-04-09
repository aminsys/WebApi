#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BokArkiv.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;

namespace BokArkiv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookContext _context;
        private static readonly string _jsonFilePath = @"Models\books.json";


        public BooksController(BookContext context)
        {
            _context = context;
            var json = System.IO.File.ReadAllText(_jsonFilePath);
            var _jObject = JsonConvert.DeserializeObject<List<Book>>(json);

            if (_context.BookItems.Any())
            {
                // Om databasen redan finns behöver vi inte initiera den varje gång vi anropar denna kontroll.
                return;
            }

            // Populerar in-memory databasen då jag inte kunde komma på ett annat
            // sätt att köra CRUD operationer på .json fil och behandla det som databas.
            foreach (var j in _jObject)
            {
                _context.BookItems.Add(new Book
                {
                    Id = j.Id,
                    Author = j.Author,
                    Title = j.Title,
                    Genre = j.Genre,
                    Price = j.Price,
                    PublishDate = j.PublishDate,
                    Description = j.Description
                });
            }

            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        // GET: api/books
        [HttpGet("/api/books")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBookItems()
        {
            return await _context.BookItems.ToListAsync();
        }

        [HttpGet("/api/books/id/{id?}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBookById(string? id = null)
        {
            if(id != null)
            {
                var book = await _context.BookItems.Where(i => i.Id.ToLower().Contains(id.ToLower())).
                                OrderBy(i => i.Id).
                                ToListAsync();

                if (book == null)
                {
                    return NotFound();
                }

                return book.ToList();
            }

            else
            {
                var book = await _context.BookItems.
                                OrderBy(i => i.Id).
                                ToListAsync();

                if (book == null)
                {
                    return NotFound();
                }

                return book.ToList();
            }
            
        }

        [HttpGet("/api/books/author/{author?}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBookByAuthor(string? author = null)
        {
            if(author != null)
            {
                var book = await _context.BookItems.Where(i => i.Author.ToLower().Contains(author.ToLower())).
                OrderBy(i => i.Author).
                ToListAsync();

                if (book == null)
                {
                    return NotFound();
                }

                return book.ToList();
            }

            else
            {
                var book = await _context.BookItems.
                                                OrderBy(i => i.Author).
                                                ToListAsync();

                if (book == null)
                {
                    return NotFound();
                }

                return book.ToList();
            }
            
        }

        [HttpGet("/api/books/title/{title?}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBookBytitle(string? title = null)
        {
            if(title != null)
            {
                var book = await _context.BookItems.Where(i => i.Title.ToLower().Contains(title.ToLower())).
                                OrderBy(i => i.Title).
                                ToListAsync();

                if (book == null)
                {
                    return NotFound();
                }

                return book.ToList();
            }
            else
            {
                var book = await _context.BookItems.
                                                OrderBy(i => i.Title).
                                                ToListAsync();

                if (book == null)
                {
                    return NotFound();
                }

                return book.ToList();
            }
            
        }

        [HttpGet("/api/books/genre/{genre?}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBookByGenre(string? genre = null)
        {
            if(genre != null)
            {
                var book = await _context.BookItems.Where(i => i.Genre.ToLower().Contains(genre.ToLower())).
                                OrderBy(i => i.Genre).
                                ToListAsync();

                if (book == null)
                {
                    return NotFound();
                }

                return book.ToList();
            }
            else
            {
                var book = await _context.BookItems.
                                OrderBy(i => i.Genre).
                                ToListAsync();

                if (book == null)
                {
                    return NotFound();
                }

                return book.ToList();
            }
            
        }

        [HttpGet("/api/books/description/{description?}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBookByDescription(string? description = null)
        {
            if(description != null)
            {
                var book = await _context.BookItems.Where(i => i.Description.ToLower().Contains(description.ToLower())).
                OrderBy(i => i.Description).
                ToListAsync();

                if (book == null)
                {
                    return NotFound();
                }

                return book.ToList();
            }
            else
            {
                var book = await _context.BookItems.
                                OrderBy(i => i.Description).
                                ToListAsync();

                if (book == null)
                {
                    return NotFound();
                }

                return book.ToList();
            }
            
        }

        // Det verkar inte som att det går att använda "&".
        // Jag fick inte till det att fungera med route:n https://host:port/api/books/price/30.0&35.0
        // Det enda som kan komma före en "optional" parameter är en punkt ".".
        // Jag använder dock en "/" istället för punkter. Exempel: /api/books/price/20/33
        // Från raden i testdokumentet: GET https://host:port/api/books/price/30.0&35.0 returns all with price between '30.0' and '35.0' sorted by price(B1, B11)
        [HttpGet("/api/books/price/{min:double?}/{max:double?}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBookByDate(double? max, double? min)
        {
            // Omöjligt att ha pris på en bok som är minus (det är som att ge pengar utöver boken).
            if(min > 0.0)
            {
                try
                {
                    // Ange antingen exakta minsta priset eller pris intervallet
                    var book = await _context.BookItems.Where(i => i.Price == min ||
                                                            (i.Price >= min &&
                                                            i.Price <= max)).
                    OrderBy(i => i.Price).
                    ToListAsync();

                    if (book == null)
                    {
                        return NotFound();
                    }

                    return book.ToList();

                }
                catch
                {
                    return NotFound();
                }
            }
            else
            {
                var book = await _context.BookItems.
                                                OrderBy(i => i.Price).
                                                ToListAsync();

                if (book == null)
                {
                    return NotFound();
                }

                return book.ToList();
            }
            
        }

        [HttpGet("/api/books/published/{year:int?}/{month:int?}/{day:int?}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBookByDate(int? year, int? month, int? day)
        {
            // Hämtar en lista som matchar datumet (yyyy/MM/dd).
            if (day > 0)
            {
                var book = await _context.BookItems.Where(i => i.PublishDate.Year == year && i.PublishDate.Month == month && i.PublishDate.Day == day).
                OrderBy(i => i.PublishDate).
                ToListAsync();

                if (book == null)
                {
                    return NotFound();
                }

                return book.ToList();
            }

            // Hämtar en lista som matchar datumet (yyyy/MM)
            else if (day == null && month > 0)
            {
                var book = await _context.BookItems.Where(i => i.PublishDate.Year == year && i.PublishDate.Month == month).
                OrderBy(i => i.PublishDate).
                ToListAsync();

                if (book == null)
                {
                    return NotFound();
                }

                return book.ToList();
            }

            // Hämtar en lista som matchar datumet (yyyy)
            else if (month == null && year > 0)
            {
                var book = await _context.BookItems.Where(i => i.PublishDate.Year == year).
                OrderBy(i => i.PublishDate).
                ToListAsync();

                if (book == null)
                {
                    return NotFound();
                }

                return book.ToList();
            }
            else
            {
                var book = await _context.BookItems.
                                OrderBy(i => i.PublishDate).
                                ToListAsync();

                if (book == null)
                {
                    return NotFound();
                }

                return book.ToList();
            }

        }

        // PUT uppdaterar objekt i databasen. Den skapar inte en ny rad enligt testdokumentet
        [HttpPut("/api/books/{id}")]
        public async Task<IActionResult> PutBook(string id, Book book)
        {
            if(id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

                Debug.WriteLine(e.Message);
            }
            return NoContent();
        }

        // POST skapar objekt i databasen. Vänligen kontrollera testdokumentet.
        [HttpPost("/api/books")]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _context.BookItems.Add(book);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BookExists(book.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBookById", book);
        }

        private bool BookExists(string id)
        {
            return _context.BookItems.Any(e => e.Id == id);
        }
    }
}
