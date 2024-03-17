using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Multilang.Context;
using Multilang.Models;

namespace Multilang.Controllers
{
    public class NewsController : Controller
    {
        private readonly MultilangDBContext _context;

        public NewsController(MultilangDBContext context)
        {
            _context = context;
        }

        // GET: News
        public async Task<IActionResult> Index()
        {
            var multilangDBContext = _context.News.Include(l=>l.Language);
            return View(await multilangDBContext.ToListAsync());
        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var @new = await _context.News
                .Include(l => l.Language)
                .FirstOrDefaultAsync(m => m.NewsId == id);
            if (@new == null)
            {
                return NotFound();
            }

            return View(@new);
        }

        // GET: News/Create
        public IActionResult Create()
        {
            ViewData["LangId"] = new SelectList(_context.Languages, "LangId", "LangTitle");
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NewsId,LangId,Title,Description,ImageName,CreateDate")] New @new,IFormFile imgNews)
        {
            if (ModelState.IsValid)
            {
                @new.CreateDate = DateTime.Now;
                if (imgNews != null)
                {
                    @new.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgNews.FileName);
                    string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/NewsImages",@new.ImageName);

                    using (var stream = new FileStream(savePath,FileMode.Create))
                    {
                        imgNews.CopyTo(stream);
                    }
                }

                // @new.LangId = 1;
                _context.Add(@new);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LangId"] = new SelectList(_context.Languages, "LangId", "LangTitle", @new.LangId);
            return View(@new);
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var @new = await _context.News.FindAsync(id);
            if (@new == null)
            {
                return NotFound();
            }
            ViewData["LangId"] = new SelectList(_context.Languages, "LangId", "LangTitle", @new.LangId);
            return View(@new);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NewsId,LangId,Title,Description,ImageName,CreateDate")] New @new)
        {
            if (id != @new.NewsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@new);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewExists(@new.NewsId))
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
            ViewData["LangId"] = new SelectList(_context.Languages, "LangId", "LangTitle", @new.LangId);
            return View(@new);
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var @new = await _context.News 
                .Include(l => l.Language)
                .FirstOrDefaultAsync(m => m.NewsId == id);
            if (@new == null)
            {
                return NotFound();
            }

            return View(@new);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.News == null)
            {
                return Problem("Entity set 'MultilangDBContext.News'  is null.");
            }
            var @new = await _context.News.FindAsync(id);
            if (@new != null)
            {
                _context.News.Remove(@new);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewExists(int id)
        {
          return (_context.News?.Any(e => e.NewsId == id)).GetValueOrDefault();
        }
    }
}
