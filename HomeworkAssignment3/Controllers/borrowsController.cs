using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using HomeworkAssignment3.Models; // Ensure this matches your actual namespace

namespace HomeworkAssignment3.Controllers
{
    public class BorrowsController : Controller
    {
        private readonly LibraryContext db = new LibraryContext(); // Use LibraryContext

        // GET: Borrows
        public async Task<ActionResult> Index()
        {
            var borrows = db.Borrows.Include(b => b.Book).Include(b => b.Student);
            return View(await borrows.ToListAsync());
        }

        // GET: Borrows/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var borrow = await db.Borrows.Include(b => b.Book).Include(b => b.Student).FirstOrDefaultAsync(b => b.BorrowId == id);
            if (borrow == null)
            {
                return HttpNotFound();
            }

            return View(borrow);
        }

        // GET: Borrows/Create
        public ActionResult Create()
        {
            ViewBag.BookId = new SelectList(db.Books, "BookId", "Name");
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "Name"); // Populate StudentId
            return View();
        }


        // POST: Borrows/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "StudentId,BookId,TakenDate,BroughtDate")] Borrow borrow)
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

        // GET: Borrows/Edit/5
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

            // Populate ViewBag.Students and ViewBag.Books before returning the view
            ViewBag.BookId = new SelectList(db.Books, "BookId", "Name", borrow.BookId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "Name", borrow.StudentId); // Populate StudentId
            return View(borrow);
        }

        // POST: Borrows/Edit/5
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

        // GET: Borrows/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var borrow = await db.Borrows.Include(b => b.Book).Include(b => b.Student).FirstOrDefaultAsync(b => b.BorrowId == id);
            if (borrow == null)
            {
                return HttpNotFound();
            }

            return View(borrow);
        }

        // POST: Borrows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var borrow = await db.Borrows.FindAsync(id);
            if (borrow != null)
            {
                db.Borrows.Remove(borrow);
                await db.SaveChangesAsync();
            }
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
