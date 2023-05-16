using CRM.Data;
using CRM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Controllers
{
    public class OrdersController : Controller
    {
        private readonly CRMContext _context;

        public OrdersController(CRMContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.Client)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToList();
            return View(orders);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            // Get the list of clients and products
            ViewBag.ClientList = new SelectList(_context.Clients, "Id", "Name");
            ViewBag.ProductList = new SelectList(_context.Products, "Id", "Name");

            // Create a new empty Order object with an empty list of OrderItems
            var order = new Order { OrderItems = new List<OrderItem>() { new OrderItem() } };

            // Pass the Order object to the view
            return View(order);
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order order)
        {
            order.Client = _context.Clients.Find(order.ClientId);
            foreach (var orderItem in order.OrderItems)
            {
                orderItem.Product = _context.Products.Find(orderItem.ProductId);
            }

            if (ModelState.IsValid)
            {
                if (_context.Orders.Find(order.Id) == null) _context.Add(order);
                else _context.Update(order);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ClientList = new SelectList(_context.Clients, "Id", "Name");
            ViewBag.ProductList = new SelectList(_context.Products, "Id", "Name");
            return View(order);
        }

        // GET: Orders/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            // Populate ViewBag with client and product lists for dropdowns
            ViewBag.ClientList = new SelectList(_context.Clients, "Id", "Name");
            ViewBag.ProductList = new SelectList(_context.Products, "Id", "Name");

            return View("Create", order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,OrderNumber,Date,ClientId,OrderItems")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update order and save changes to database
                    _context.Update(order);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Orders.Find(order.Id) != null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // If model state is not valid, repopulate dropdown lists and return to Edit view
            ViewBag.ClientList = new SelectList(_context.Clients, "Id", "Name");
            ViewBag.ProductList = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

    }
}
