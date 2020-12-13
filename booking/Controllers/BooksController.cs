using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingMS.Models;
using System.Linq;
using Microsoft.AspNetCore.Cors;

namespace BookingMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	[EnableCors("AllowOrigin")]
    public class BooksController : ControllerBase
    {
        private readonly Library_DbContext _context;

        public BooksController(Library_DbContext context)
        {
            _context = context;
        }

        // GET: api/Book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBook()
        {
            //next line of code fetches all books details from db
            return await _context.Books.ToListAsync();
        }

        [HttpGet]
        [Route("HealthCheck")]
        public async Task<string> HealthCheck()
        {
            //next line of code fetches all books details from db
            try
            {
                var result = await _context.Books.ToListAsync();
                return "Success";
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
        }

        [HttpGet]
        [Route("Hello")]
        public ActionResult<string> Hello()
        {
            //next line of code fetches all books details from db
            return "Hello";
        }

        // GET: api/Book/GetBooksByCategoryName
        [HttpGet]
        [Route("GetBooksByCategoryName")]
        public ActionResult GetBooksByCategoryName(string name)
        {
            //next line of code fetches book details for the category from db
            var details = from b in _context.Books
                            join c in _context.Categories on b.CatId equals c.CatId
                          where (string.IsNullOrEmpty(name) || c.Name.Contains(name))
                            select new
                            {
                                BookTitle = b.Title,
                                CategoryName = c.Name
                            };
            return Ok(details);
        }

        [HttpGet]
        [Route("GetAllCategories")]
        public IEnumerable<Category> GetAllCategories()
        {
            //next line of code fetches all category details from db
            return _context.Categories.ToList();
        }

        [HttpPost]
        [Route("GetBooksByBookCodes")]
        public ActionResult<List<Book>> GetBooksByBookCodes(string[] bookCodes)
        {
            //next line of code fetches books details corresponding to the id from db
            var books =  _context.Books.Where(x => bookCodes.Contains(x.BookCode)).ToList();

            if (books == null)
            {
                return NotFound();
            }

            return books;
        }

        // GET: api/Book/5
        // 
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            //next line of code fetches books details corresponding to the id from db
            var Book = await _context.Books.FindAsync(id);

            if (Book == null)
            {
                return NotFound();
            }

            return Book;
        }

        // GET: api/Book/5
        // 
        [HttpGet]
        [Route("GetBookByNameAndAuthor")]
        public ActionResult<List<Book>> GetBookByNameAndAuthor(string name, string author)
        {
            //next line of code fetches books details corresponding to the given name and author from db
            var Book = _context.Books.Where(x => (string.IsNullOrEmpty(name) || x.Title.Contains(name))
                                                  && (string.IsNullOrEmpty(author) || x.Author.Contains(author))).ToList();

            if (Book == null)
            {
                return NotFound();
            }

            return Book;
        }

        [HttpGet]
        [Route("GetBookByNameOrAuthor")]
        public ActionResult<List<Book>> GetBookByNameOrAuthor(string name, string author)
        {
            //next line of code fetches books details corresponding to the given name or author from db
            var Book = _context.Books.Where(x => (string.IsNullOrEmpty(name) || x.Title.Contains(name))
                                                  || (string.IsNullOrEmpty(author) || x.Author.Contains(author))).ToList();

            if (Book == null)
            {
                return NotFound();
            }

            return Book;
        }

        [HttpGet]
        [Route("SearchBook")]
        public ActionResult<List<Book>> SearchBook(string search)
        {
            //next line of code fetches books details if there is a match for Title,Author,Description or Publisher from db
            var Book = _context.Books.Where(x => (string.IsNullOrEmpty(search) || x.Title.Contains(search))
                                                  || (string.IsNullOrEmpty(search) || x.Author.Contains(search))
                                                  || (string.IsNullOrEmpty(search) || x.Description.Contains(search))
                                                  || (string.IsNullOrEmpty(search) || x.Publisher.Contains(search))).ToList();

            if (Book == null)
            {
                return NotFound();
            }

            return Book;
        }

        // PUT: api/Book/5
        /// <summary>
        ///  used to update the user data
        /// </summary>
        /// <param name="Book"></param>
        /// <returns>bool</returns>
        [HttpPut]
        public bool PutBook(Book Book)
        {
            if (BookExists(Book.BookId))
            {
                //update the book data corresponding to the id
                _context.Books.Update(Book);
            }
            else
            {
                return false;
            }
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;

            }

            return true;
        }

        // POST: api/Book
        [HttpPost]
        public ActionResult<Book> PostBook(Book Book)
        {
            //next line of code creates an entry of Book in the book table
            _context.Books.Add(Book);
            var result = _context.SaveChanges();

            return CreatedAtAction("GetBook", new { id = result }, Book);
        }

        // DELETE: api/Book/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            //next line of code updates the data corresponding to the id in the book table
            var Book = await _context.Books.FindAsync(id);
            if (Book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(Book);
            await _context.SaveChangesAsync();

            return Book;
        }

        private bool BookExists(int id)
        {
            //next line of code checks whether the data exists for the given id 
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
