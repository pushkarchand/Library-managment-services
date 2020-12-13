using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderMS.Models;
using System.Linq;

namespace OrderMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")] 
    public class OrdersController : ControllerBase
    {
        private readonly Library_DbContext _context;

        public OrdersController(Library_DbContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            //next line of code fetches all Orders details from db
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Order/5
        // 
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            //next line of code fetches Orders details corresponding to the id from db
            var Order = await _context.Orders.FindAsync(id);

            if (Order == null)
            {
                return NotFound();
            }

            return Order;
        }

        // GET: api/Order/abc1
        // 
        [HttpGet]
        [Route("GetOrderByUserCode")]
        public ActionResult<List<Order>> GetOrderByUserCode(string userCode)
        {
            //next line of code fetches Loans details corresponding to the id from db
            var orders = _context.Orders.Where(x => x.UserCode == userCode).ToList();

            if (orders == null)
            {
                return NotFound();
            }

            return orders;
        }

        // PUT: api/Order/5
        /// <summary>
        ///  used to update the user data
        /// </summary>
        /// <param name="Order"></param>
        /// <returns>bool</returns>
        [HttpPut]
        public bool PutOrder(Order Order)
        {
            if (OrderExists(Order.OrderId))
            {
                //update the Order data corresponding to the id
                _context.Orders.Update(Order);
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

        // POST: api/Order
        [HttpPost]
        public ActionResult<Order> PostOrder(Order Order)
        {
            //next line of code creates an entry of Order in the Order table
            _context.Orders.Add(Order);
            var result = _context.SaveChanges();

            return CreatedAtAction("GetOrder", new { id = result }, Order);
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            //next line of code updates the data corresponding to the id in the Order table
            var Order = await _context.Orders.FindAsync(id);
            if (Order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(Order);
            await _context.SaveChangesAsync();

            return Order;
        }

        private bool OrderExists(int id)
        {
            //next line of code checks whether the data exists for the given id 
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
