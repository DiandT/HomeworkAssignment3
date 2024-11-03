using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using HomeworkAssignment3;

namespace HomeworkAssignment3.Controllers
{
    public class TypesController : Controller
    {
        private readonly LibraryEntities db = new LibraryEntities();

        // GET: Types
        public async Task<ActionResult> Index()
        {
            return View(await db.types.ToListAsync());
        }

        // GET: Types/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var type = await db.types.FindAsync(id);
            if (type == null)
            {
                return HttpNotFound();
            }

            return View(type);
        }

        // GET: Types/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Types/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "typeId,name")] type type)
        {
            if (ModelState.IsValid)
            {
                db.types.Add(type);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(type);
        }

        // GET: Types/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var type = await db.types.FindAsync(id);
            if (type == null)
            {
                return HttpNotFound();
            }

            return View(type);
        }

        // POST: Types/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "typeId,name")] type type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(type).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(type);
        }

        // GET: Types/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var type = await db.types.FindAsync(id);
            if (type == null)
            {
                return HttpNotFound();
            }

            return View(type);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var type = await db.types.FindAsync(id);
                if (type == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Type record could not be found.");
                }

                db.types.Remove(type);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (use your preferred logging approach)
                Console.WriteLine(ex.Message); // or use a logging library
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error deleting the record.");
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
