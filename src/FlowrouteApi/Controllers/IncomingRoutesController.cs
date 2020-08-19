using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlowrouteApi.DataModels;
using Microsoft.AspNetCore.Authorization;

namespace FlowrouteApi.Controllers
{
    public class IncomingRoutesController : Controller
    {
        private readonly DataContext _context;

        public IncomingRoutesController(DataContext context)
        {
            _context = context;
        }

        // GET: IncomingRoutes
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.IncomingRoutes.ToListAsync());
        }

        // GET: IncomingRoutes/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incomingRoute = await _context.IncomingRoutes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incomingRoute == null)
            {
                return NotFound();
            }

            return View(incomingRoute);
        }

        // GET: IncomingRoutes/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: IncomingRoutes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Phone,Email")] IncomingRoute incomingRoute)
        {
            if (ModelState.IsValid)
            {
                _context.Add(incomingRoute);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(incomingRoute);
        }

        // GET: IncomingRoutes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incomingRoute = await _context.IncomingRoutes.FindAsync(id);
            if (incomingRoute == null)
            {
                return NotFound();
            }
            return View(incomingRoute);
        }

        // POST: IncomingRoutes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Phone,Email")] IncomingRoute incomingRoute)
        {
            if (id != incomingRoute.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incomingRoute);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncomingRouteExists(incomingRoute.Id))
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
            return View(incomingRoute);
        }

        // GET: IncomingRoutes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incomingRoute = await _context.IncomingRoutes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incomingRoute == null)
            {
                return NotFound();
            }

            return View(incomingRoute);
        }

        // POST: IncomingRoutes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incomingRoute = await _context.IncomingRoutes.FindAsync(id);
            _context.IncomingRoutes.Remove(incomingRoute);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncomingRouteExists(int id)
        {
            return _context.IncomingRoutes.Any(e => e.Id == id);
        }
    }
}
