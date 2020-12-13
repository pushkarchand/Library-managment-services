using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoanMS.Models;
using System.Linq;
using Microsoft.AspNetCore.Cors;

namespace LoanMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class LoansController : ControllerBase
    {
        private readonly Library_DbContext _context;

        public LoansController(Library_DbContext context)
        {
            _context = context;
        }

        // GET: api/Loan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoan()
        {
            //next line of code fetches all Loans details from db
            return await _context.Loans.ToListAsync();
        }

        // GET: api/Loan/5
        // 
        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetLoan(int id)
        {
            //next line of code fetches Loans details corresponding to the id from db
            var Loan = await _context.Loans.FindAsync(id);

            if (Loan == null)
            {
                return NotFound();
            }

            return Loan;
        }

        // GET: api/Loan/abc1
        // 
        [HttpGet]
        [Route("GetLoanByUserCode")]
        public ActionResult<List<Loan>> GetLoanByUserCode(string userCode)
        {
            //next line of code fetches Loans details corresponding to the id from db
            var Loan = _context.Loans.Where(x=>x.UserCode == userCode).ToList();

            if (Loan == null)
            {
                return NotFound();
            }

            return Loan;
        }

        // PUT: api/Loan/5
        /// <summary>
        ///  used to update the user data
        /// </summary>
        /// <param name="Loan"></param>
        /// <returns>bool</returns>
        [HttpPut]
        public bool PutLoan(Loan Loan)
        {
            if (LoanExists(Loan.LoanId))
            {
                //update the Loan data corresponding to the id
                _context.Loans.Update(Loan);
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

        // POST: api/Loan
        [HttpPost]
        public ActionResult<Loan> PostLoan(Loan Loan)
        {
            //next line of code creates an entry of Loan in the Loan table
            _context.Loans.Add(Loan);
            var result = _context.SaveChanges();

            return CreatedAtAction("GetLoan", new { id = result }, Loan);
        }

        // DELETE: api/Loan/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Loan>> DeleteLoan(int id)
        {
            //next line of code updates the data corresponding to the id in the Loan table
            var Loan = await _context.Loans.FindAsync(id);
            if (Loan == null)
            {
                return NotFound();
            }

            _context.Loans.Remove(Loan);
            await _context.SaveChangesAsync();

            return Loan;
        }

        private bool LoanExists(int id)
        {
            //next line of code checks whether the data exists for the given id 
            return _context.Loans.Any(e => e.LoanId == id);
        }
    }
}
