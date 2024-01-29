using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Models;

namespace PMS.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var currentUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var applicationDbContext = _context.Project.Where(t => t.EmployeeId == currentUser).Include(t => t.Employee).Include(t => t.Status);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .Include(p => p.Employee)
                .Include(p => p.Tasks).ThenInclude(t => t.Status)
                .Include(p => p.Tasks).ThenInclude(t => t.Priority)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        //[Authorize(Roles="Administrator,Manager")]
        public IActionResult Create()
        {
            var currentUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewData["EmployeeId"] = new SelectList(_context.Set<Employee>().Where(i => i.Id == currentUser), "Id", "UserName");
            ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Name");

            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrator,Manager")]

        public async Task<IActionResult> Create([Bind("Name,StartDate,EndDate,Description,EmployeeId,StatusId")] Project project)
        {
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                project.Id = Guid.NewGuid().ToString();
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Set<Employee>(), "Id", "UserName", project.EmployeeId);
            return View(project);
        }

        // GET: Projects/Edit/5
        //[Authorize(Roles = "Administrator,Manager")]

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Set<Employee>(), "Id", "UserName");
            ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Name");

            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Manager")]

        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,StartDate,EndDate,StatusId,Description,EmployeeId")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Set<Employee>(), "Id", "Id", project.EmployeeId);
            ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Name", project.StatusId);

            return View(project);
        }

        // GET: Projects/Delete/5
        //[Authorize(Roles = "Administrator,Manager")]

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrator,Manager")]

        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var project = await _context.Project.FindAsync(id);
            if (project != null)
            {
                _context.Project.Remove(project);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(string id)
        {
            return _context.Project.Any(e => e.Id == id);
        }
    }
}
