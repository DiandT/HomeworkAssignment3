using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using HomeworkAssignment3.Models; // Make sure this namespace is correct

namespace HomeworkAssignment3.Controllers
{
    public class BooksController : Controller // Correct the naming convention for the controller class
    {
        private LibraryContext db = new LibraryContext();

        // GET: Books
        public async Task<ActionResult> Index()
        {
            var books = db.Books.Include(b => b.Author).Include(b => b.Type); // Adjust casing for DbSet properties
            return View(await books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var book = await db.Books.FindAsync(id); // Adjust casing for DbSet properties
            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Authors, "AuthorId", "Name"); // Adjust casing for DbSet properties
            ViewBag.TypeId = new SelectList(db.Types, "TypeId", "Name"); // Adjust casing for DbSet properties
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BookId,Name,PageCount,Point,AuthorId,TypeId")] Book book) // Adjust to the correct model type
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book); // Adjust casing for DbSet properties
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(db.Authors, "AuthorId", "Name", book.AuthorId); // Adjust casing for DbSet properties
            ViewBag.TypeId = new SelectList(db.Types, "TypeId", "Name", book.TypeId); // Adjust casing for DbSet properties
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var book = await db.Books.FindAsync(id); // Adjust casing for DbSet properties
            if (book == null)
            {
                return HttpNotFound();
            }

            ViewBag.AuthorId = new SelectList(db.Authors, "AuthorId", "Name", book.AuthorId); // Adjust casing for DbSet properties
            ViewBag.TypeId = new SelectList(db.Types, "TypeId", "Name", book.TypeId); // Adjust casing for DbSet properties
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BookId,Name,PageCount,Point,AuthorId,TypeId")] Book book) // Adjust to the correct model type
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(db.Authors, "AuthorId", "Name", book.AuthorId); // Adjust casing for DbSet properties
            ViewBag.TypeId = new SelectList(db.Types, "TypeId", "Name", book.TypeId); // Adjust casing for DbSet properties
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var book = await db.Books.FindAsync(id); // Adjust casing for DbSet properties
            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var book = await db.Books.FindAsync(id); // Adjust casing for DbSet properties
            db.Books.Remove(book); // Adjust casing for DbSet properties
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
