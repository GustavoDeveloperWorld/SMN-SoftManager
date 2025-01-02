using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoftManager.Domain.Entities;
using SoftManager.Infrastructure.Persistence.Contexts;

namespace SoftManager.Presentation.Controllers
{
    [Authorize]
    public class UserTasksController : Controller
    {
        private readonly InfraDbContext _context;

        public UserTasksController(InfraDbContext context)
        {
            _context = context;
        }

        // GET: UserTasks
        public async Task<IActionResult> Index()
        {
            var infraDbContext = _context.UserTasks.Include(u => u.ApplicationUser);
            return View(await infraDbContext.ToListAsync());
        }

        // GET: UserTasks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTask = await _context.UserTasks
                .Include(u => u.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userTask == null)
            {
                return NotFound();
            }

            return View(userTask);
        }

        [Authorize(Policy = "ManagerOnly")]
        // GET: UserTasks/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: UserTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Task,Message,DueDate,ApplicationUserId,IsCompleted,CompletionDate")] UserTask userTask)
        {
            userTask.ApplicationUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userTask.ApplicationUserId);

            if (userTask.ApplicationUser == null)
            {
                ModelState.AddModelError("ApplicationUserId", "Usuário não encontrado.");
            }

            if (ModelState.IsValid)
            {
                userTask.Id = Guid.NewGuid();
                _context.Add(userTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "UserName", userTask.ApplicationUserId);
            return View(userTask);
        }


        // GET: UserTasks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTask = await _context.UserTasks.FindAsync(id);
            if (userTask == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "UserName", userTask.ApplicationUserId);
            return View(userTask);
        }

        // POST: UserTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Task,Message,DueDate,ApplicationUserId,IsCompleted,CompletionDate")] UserTask userTask)
        {
            if (id != userTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserTaskExists(userTask.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "UserName", userTask.ApplicationUserId);
            return View(userTask);
        }

        // GET: UserTasks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTask = await _context.UserTasks
                .Include(u => u.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userTask == null)
            {
                return NotFound();
            }

            return View(userTask);
        }

        // POST: UserTasks/Delete/5
        [Authorize(Policy = "ManagerOnly")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userTask = await _context.UserTasks.FindAsync(id);
            if (userTask != null)
            {
                _context.UserTasks.Remove(userTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserTaskExists(Guid id)
        {
            return _context.UserTasks.Any(e => e.Id == id);
        }
    }
}
