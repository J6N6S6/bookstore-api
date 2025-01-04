using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bookstore.Data;
using Bookstore.DTOs;
using Bookstore.Models;
using Microsoft.AspNetCore.Authorization;

namespace Bookstore.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    [ApiController]
    [Route("api/[Controller]")]
    public class BookstoreController : ControllerBase
    {
        private readonly BookstoreDbContext _db;

        public BookstoreController(BookstoreDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookstore()
        {
            var bookstores = await _db.Bookstores.ToListAsync();
            return Ok(bookstores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookstoreById(int id)
        {
            var bookstore = await _db.Bookstores.FindAsync(id);

            if (bookstore == null)
            {
                return NotFound("Livraria não encontrada.");
            }

            return Ok(new BookstoresDTO
            {
                Name = bookstore.Name,
                Location = bookstore.Location
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateBookstore([FromBody] BookstoresDTO bookstores)
        {
            if (_db.Bookstores.Any(bs => bs.Name == bookstores.Name))
            {
                return BadRequest("Livraria já cadastrada.");
            }

            var newBookstore = new Bookstores
            {
                Name = bookstores.Name,
                Location = bookstores.Location
            };

            _db.Bookstores.Add(newBookstore);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookstoreById), new { id = newBookstore.Id },
                new BookstoresDTO
                {
                    Name = bookstores.Name,
                    Location = bookstores.Location
                });
        }
    }
}
