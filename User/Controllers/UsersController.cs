using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User.Models;

namespace User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Library_DbContext _context;

        public UsersController(Library_DbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            //next line of code fetches all users details from db
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        // 
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
            //next line of code fetches user details corresponding to the id from db
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        // PUT: api/Users/5
        /// <summary>
        ///  used to update the user data
        /// </summary>
        /// <param name="users"></param>
        /// <returns>bool</returns>
        [HttpPut]
        public bool PutUsers(Users users)
        {
            //next line of code checks whether the data exists for the given id 
            if (UsersExists(users.UserId))
            {
                ////update the user data corresponding to the id
                _context.Users.Update(users);
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

        // POST: api/Users
        [HttpPost]
        public ActionResult<Users> PostUsers(Users users)
        {
            //next line of code creates an entry of user in the user table
            _context.Users.Add(users);
            var result = _context.SaveChanges();

            return CreatedAtAction("GetUsers", new { id = result }, users);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Users>> DeleteUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return users;
        }

        [HttpGet]
        [Route("ValidateUserLogin")]
        //this is an api
        public ActionResult<LoginResponse> ValidateUserLogin(string email, string password)
        {
            if (_context.Users.Any(e => (e.Email == email && e.Password == password)))
            {
                var user = _context.Users.Where(e => (e.Email == email && e.Password == password)).FirstOrDefault();
                var result = new LoginResponse { IsValid = true, UserId = user.UserId, UserCode = user.UserName, RoleType = user.RoleType };
                return result;
            }
            else
            {
                return new LoginResponse { IsValid = false };
            }
        }

        //this is a private function
        private bool UsersExists(int id)
        {
            // next line of code checks whether the data exists for the given id
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
