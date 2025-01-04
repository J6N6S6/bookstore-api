using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bookstore.Data;
using Bookstore.DTOs;
using Bookstore.Models;
using Microsoft.AspNetCore.Authorization;

namespace Bookstore.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookstoreDbContext _context;

        public BooksController(BookstoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _context.Books.OrderBy(b => b.Title).ToListAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound("Livro não encontrado.");
            }

            return Ok(new BookDTO
            {
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                BookstoreId = book.BookstoreId,
                Stock = book.Stock
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookDTO book)
        {
            if (_context.Books.Any(b => b.Title == book.Title))
            {
                return BadRequest("Livro já cadastrado.");
            }

            var newBook = new Book
            {
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                BookstoreId = book.BookstoreId,
                Stock = book.Stock
            };

            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id },
                new BookDTO
                {
                    Title = newBook.Title,
                    Author = newBook.Author,
                    Price = newBook.Price,
                    BookstoreId = newBook.BookstoreId,
                    Stock = newBook.Stock
                });
        }
    }
}
