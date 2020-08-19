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
    public class OutgoingRoutesController : Controller
    {
        private readonly DataContext _context;

        public OutgoingRoutesController(DataContext context)
        {
            _context = context;
        }

        // GET: OutgoingRoutes
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.OutgoingRoutes.ToListAsync());
        }

        // GET: OutgoingRoutes/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outgoingRoute = await _context.OutgoingRoutes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (outgoingRoute == null)
            {
                return NotFound();
            }

            return View(outgoingRoute);
        }

        // GET: OutgoingRoutes/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: OutgoingRoutes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Email,Phone")] OutgoingRoute outgoingRoute)
        {
            if (ModelState.IsValid)
            {
                _context.Add(outgoingRoute);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(outgoingRoute);
        }

        // GET: OutgoingRoutes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outgoingRoute = await _context.OutgoingRoutes.FindAsync(id);
            if (outgoingRoute == null)
            {
                return NotFound();
            }
            return View(outgoingRoute);
        }

        // POST: OutgoingRoutes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Phone")] OutgoingRoute outgoingRoute)
        {
            if (id != outgoingRoute.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(outgoingRoute);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OutgoingRouteExists(outgoingRoute.Id))
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
            return View(outgoingRoute);
        }

        // GET: OutgoingRoutes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outgoingRoute = await _context.OutgoingRoutes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (outgoingRoute == null)
            {
                return NotFound();
            }

            return View(outgoingRoute);
        }

        // POST: OutgoingRoutes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var outgoingRoute = await _context.OutgoingRoutes.FindAsync(id);
            _context.OutgoingRoutes.Remove(outgoingRoute);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OutgoingRouteExists(int id)
        {
            return _context.OutgoingRoutes.Any(e => e.Id == id);
        }
    }
}
