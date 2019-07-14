using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Owl.Data;
using Owl.Models;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using HeyRed.MarkdownSharp;
using Ganss.XSS;
using Microsoft.AspNetCore.Authorization;


namespace Owl.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public static string GetGravatar(string email, string size = "200")
        {
            MD5 md5 = MD5.Create();
            byte[] md5Data = md5.ComputeHash(Encoding.UTF8.GetBytes(email));
            StringBuilder emailBuilder = new StringBuilder();
            foreach (var x in md5Data)
            {
                emailBuilder.Append(x.ToString("x2"));
            }
            string emailString = emailBuilder.ToString();

            return $"https://www.gravatar.com/avatar/{emailString}?s={size}";
           // }
        }

        // GET: Home
        public async Task<IActionResult> Index()
        {
            var Posts = _context.Post.Where(x => x.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value).ToListAsync();
            return View(await Posts);
        }

        // GET: /Recent
        public async Task<IActionResult> Recent()
        {
            var Posts = _context.Post.Take(10).ToListAsync();
            return View(await Posts);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            var email = _context.Users.First(x => x.Id == post.UserId).Email;
            post.UserEmail = GetGravatar(email, "60");
            post.PostData = MarkdownPost(post.PostData);
            post.PostMessages = _context.Message.Where(x => x.PostId == post.Id).ToList();
            post.PostMessages.ForEach(
                x => x.MessageData = MarkdownPost(x.MessageData));
            post.PostMessages.ForEach(
                x => x.UserEmail = GetGravatar(_context.Users.First(i => i.Id == x.UserId).Email, "50"));
            //post.PostData = markdown.Transform(post.PostData)
            return View(post);

        }

        public static string MarkdownPost(string data)
        {
            var markdown = new Markdown();
            var sanitize = new HtmlSanitizer();
            //sanitize.remo
            data = sanitize.Sanitize(markdown.Transform(data));
            return data;
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostTitle,PostData")] Post post)
            // id, userid, postdate
        {
            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                post.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                post.PostDate = DateTime.Now;
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToPage(nameof(Create));
            }
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostTitle,PostData")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (post.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                    {
                        _context.Update(post);
                        await _context.SaveChangesAsync();
                    }   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
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
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            if (post.UserId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                return RedirectToAction(nameof(Index));
            }
            post.PostData = MarkdownPost(post.PostData);

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post.UserId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                return RedirectToAction(nameof(Index));
            }
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
    }
}
