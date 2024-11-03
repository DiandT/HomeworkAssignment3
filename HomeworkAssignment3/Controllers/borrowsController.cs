using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HomeworkAssignment3.Models; // Ensure this matches your actual namespace
using HomeworkAssignment3; // If you need other entities

namespace HomeworkAssignment3.Controllers
{
    public class borrowsController : Controller
    {
        private LibraryContext db = new LibraryContext(); // Use LibraryContext instead

        // GET: borrows
        public async Task<ActionResult> Index()
        {
            var borrows = db.Borrows.Include(b => b.Book).Include(b => b.Student);
            return View(await borrows.ToListAsync());
        }

        // GET: borrows/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrow borrow = await db.Borrows.FindAsync(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            return View(borrow);
        }

        // GET: borrows/Create
        public ActionResult Create()
        {
            ViewBag.BookId = new SelectList(db.Books, "BookId", "Name");
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "Name");
            return View();
        }

        // POST: borrows/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BorrowId,StudentId,BookId,TakenDate,BroughtDate")] Borrow borrow)
        {
            if (ModelState.IsValid)
            {
                db.Borrows.Add(borrow);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BookId = new SelectList(db.Books, "BookId", "Name", borrow.BookId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "Name", borrow.StudentId);
            return View(borrow);
        }

        // GET: borrows/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrow borrow = await db.Borrows.FindAsync(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookId = new SelectList(db.Books, "BookId", "Name", borrow.BookId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "Name", borrow.StudentId);
            return View(borrow);
        }

        // POST: borrows/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BorrowId,StudentId,BookId,TakenDate,BroughtDate")] Borrow borrow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(borrow).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BookId = new SelectList(db.Books, "BookId", "Name", borrow.BookId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "Name", borrow.StudentId);
            return View(borrow);
        }

        // GET: borrows/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrow borrow = await db.Borrows.FindAsync(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            return View(borrow);
        }

        // POST: borrows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Borrow borrow = await db.Borrows.FindAsync(id);
            db.Borrows.Remove(borrow);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
