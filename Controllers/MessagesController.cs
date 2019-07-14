using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Owl.Data;
using Owl.Models;

namespace Owl.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Messages
        public async Task<IActionResult> Index()
        {
            return View(await _context.Message.ToListAsync());
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Messages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,MessageData")] Message message) // id, postid, userid, messagedate
        {
            if (ModelState.IsValid)
            {
                message.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                message.MessageDate = DateTime.Now;
                _context.Add(message);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Posts", new { id = message.PostId });
        }

        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id, MessageData")] Message message)
        {
            var newData = message.MessageData;
            message = await _context.Message.FindAsync(id);
            if (id != message.Id)
            {
                return Json(new { error = "message id aint the same" });
            }
            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != message.UserId)
            {
                return Json(new { error = "youre not the user" });
            }


            if (ModelState.IsValid)
            {
                try
                {
                    message.MessageDate = DateTime.Now;
                    message.MessageData = newData;
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.Id))
                    {
                        return Json(new { error = "message doesnt exist, i guess" });
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction("Details", "Posts", message.PostId);
        }

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<JsonResult> DeleteConfirmed(int id)
        {
            var message = await _context.Message.FindAsync(id);
            if (User.FindFirst(ClaimTypes.NameIdentifier).Value == message.UserId)
            {
                _context.Message.Remove(message);
                await _context.SaveChangesAsync();
                return Json(message);//RedirectToAction(nameof(Index));
            }
            else
            {
                return Json(new { error = "LOL." });
            }
        }

        private bool MessageExists(int id)
        {
            return _context.Message.Any(e => e.Id == id);
        }
    }
}
