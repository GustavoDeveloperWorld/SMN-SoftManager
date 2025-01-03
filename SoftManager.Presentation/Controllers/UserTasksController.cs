using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoftManager.Domain.Entities;
using SoftManager.Domain.Interfaces;
using SoftManager.Infrastructure.Persistence.Contexts;

namespace SoftManager.Presentation.Controllers
{
    [Authorize]
    public class UserTasksController : Controller
    {
        private readonly InfraDbContext _context;
        private readonly IEmailService _emailService;
        public UserTasksController(IEmailService emailService, InfraDbContext context)
        {
            _emailService = emailService;
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

                string subject = "Nova Tarefa Atribuída";
                string body = $"Olá {userTask.ApplicationUser.UserName}, uma nova tarefa foi atribuída a você: {userTask.Task}.";
                await _emailService.SendEmailAsync(userTask.ApplicationUser.Email, subject, body);

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
                    var existingTask = await _context.UserTasks
                        .Include(t => t.ApplicationUser)
                        .FirstOrDefaultAsync(t => t.Id == id);

                    if (existingTask == null)
                    {
                        return NotFound();
                    }

                    existingTask.Task = userTask.Task;
                    existingTask.Message = userTask.Message;
                    existingTask.DueDate = userTask.DueDate;
                    existingTask.ApplicationUserId = userTask.ApplicationUserId;

                    bool taskCompleted = !existingTask.IsCompleted && userTask.IsCompleted;
                    existingTask.IsCompleted = userTask.IsCompleted;
                    existingTask.CompletionDate = userTask.IsCompleted ? DateTime.UtcNow : (DateTime?)null;

                    _context.Update(existingTask);
                    await _context.SaveChangesAsync();

                    if (!taskCompleted)
                    {
                        string subordinateSubject = "Tarefa Atualizada";
                        string subordinateBody = $"Olá {existingTask.ApplicationUser.UserName}, a tarefa '{existingTask.Task}' foi atualizada. Verifique os detalhes no sistema.";
                        await _emailService.SendEmailAsync(existingTask.ApplicationUser.Email, subordinateSubject, subordinateBody);
                    }

                    IList<ApplicationUser> managers = _context.Users.Where(u => u.IsManager == true).ToList();

                    foreach (var manager in managers)
                    {
                        if (manager != null && !taskCompleted)
                        {
                            string managerSubject = "Tarefa Atualizada";
                            string managerBody = $"A tarefa '{existingTask.Task}' atribuída a {existingTask.ApplicationUser.UserName} foi atualizada.";
                            await _emailService.SendEmailAsync(manager.Email, managerSubject, managerBody);
                        }

                        if (taskCompleted && manager != null)
                        {
                            string managerCompletionSubject = "Tarefa Concluída";
                            string managerCompletionBody = $"A tarefa '{existingTask.Task}' atribuída a {existingTask.ApplicationUser.UserName} foi concluída.";
                            await _emailService.SendEmailAsync(manager.Email, managerCompletionSubject, managerCompletionBody);
                        }
                    }

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
