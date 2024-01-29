using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PMS.Data;
using PMS.Models;

namespace PMS.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var currentUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var applicationDbContext = _context.Task.Where(t => t.EmployeeId == currentUser).Include(t=>t.Status).Include(t=>t.Priority).Include(t => t.Employee).Include(t => t.Project);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .Include(t => t.Employee)
                .Include(t => t.Project)
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Include (t => t.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }


        /* public IActionResult Create()
         {
             ViewData["EmployeeId"] = new SelectList(_context.Set<Employee>(), "Id", "UserName");
             ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Name");
             ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Name");
             ViewData["PriorityId"] = new SelectList(_context.Set<Priority>(), "Id", "Name");

             return View();
         }*/

        // GET: Tasks/Create --> specific project
        public IActionResult Create(string projectId)
        {
            if (projectId.IsNullOrEmpty())
            {
                return RedirectToAction("Index");
            }
            var currentUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ViewData["EmployeeId"]  = new SelectList(_context.Set<Employee>().Where(i=>i.ManagerId == currentUser), "Id", "UserName");
            ViewData["ProjectId"]   = new SelectList(_context.Project.Where(i => i.Id == projectId), "Id", "Name");
            ViewData["StatusId"]    = new SelectList(_context.Set<Status>(), "Id", "Name");
            ViewData["PriorityId"]  = new SelectList(_context.Set<Priority>(), "Id", "Name");

            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,StartDate,EndDate,EmployeeId,ProjectId,PriorityId,StatusId")] PMS.Models.Task task)
        {
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                task.Id = Guid.NewGuid().ToString();
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["EmployeeId"] = new SelectList(_context.Set<Employee>(), "Id", "UserName", task.EmployeeId);
            ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Name", task.ProjectId);
            ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Name",task.StatusId);
            ViewData["PriorityId"] = new SelectList(_context.Set<Priority>(), "Id", "Name",task.PriorityId);


            return View(task);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        /*public async Task<IActionResult> AddComment([Bind("Id,comment,TaskId,EmployeeId")] PMS.Models.Comment comment)//[Bind("Id,comment,TaskId,EmployeeId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }*/

        public async Task<IActionResult> AddComment(IFormCollection form)
        {
            Comment comment = new Comment
            {
                Id = Guid.NewGuid().ToString(),
                CommentContent = form["comment"],
                TaskId = form["TaskId"],
                EmployeeId = form["EmployeeId"],
                CreatedAt = DateTime.Now
            };
            _context.Add(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

/*        public async Task<IActionResult> CreateComment(string TaskId, string EmployeeId, Comment comment)
        {

            return View();
        }*/
        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Set<Employee>(), "Id", "UserName", task.EmployeeId);
            ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Name", task.ProjectId);
            ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Name",task.StatusId);
            ViewData["PriorityId"] = new SelectList(_context.Set<Priority>(), "Id", "Name",task.PriorityId);


            return View(task);
        }
        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Description,PriorityId,StartDate,EndDate,EmployeeId,ProjectId,StatusId")] PMS.Models.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Set<Employee>(), "Id", "UserName", task.EmployeeId);
            ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Name", task.ProjectId);
            ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Name",task.StatusId);
            ViewData["PriorityId"] = new SelectList(_context.Set<Priority>(), "Id", "Name",task.PriorityId);


            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task
            .Include(t => t.Employee)
            .Include(t => t.Project)
            .Include(t => t.Status)
            .Include(t => t.Priority)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var task = await _context.Task.FindAsync(id);
            if (task != null)
            {
                _context.Task.Remove(task);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(string id)
        {
            return _context.Task.Any(e => e.Id == id);
        }
    }
}
