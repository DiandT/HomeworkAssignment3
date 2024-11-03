using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.Entity;
using HomeworkAssignment3.Models; // Adjust this namespace as per your project
using PagedList;
using System.Collections.Generic;

public class HomeController : Controller
{
    private readonly LibraryContext _context;

    public HomeController()
    {
        _context = new LibraryContext();
    }

    public ActionResult Reports()
    {
        return View();
    }

    // Combined Index View for Students and Books with pagination
    public async Task<ActionResult> Index(int page = 1, int page2 = 1)
    {
        int pageSize = 10; // Set the number of records per page

        // Pagination for Students
        var students = await _context.Students
            .OrderBy(s => s.Name) // Order by Name or relevant property
            .ToListAsync();

        var pagedStudents = students.ToPagedList(page, pageSize);

        // Pagination for Books with Status
        var books = await _context.Books.ToListAsync();
        var bookStatus = books.Select(b => new BookStatusViewModel
        {
            Book = b,
            Status = _context.Borrows.Any(br => br.BookId == b.BookId) ? "Available" : "Out"
        })
        .OrderBy(b => b.Book.Name)
        .ToList();

        var pagedBooks = bookStatus.ToPagedList(page2, pageSize);

        // Populate the ViewModel
        var viewModel = new LibraryHomeViewModel
        {
            Students = pagedStudents,
            BookStatus = pagedBooks,
            NewStudent = new Student(),
            NewBook = new Book()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult> CreateStudent(Student student)
    {
        if (ModelState.IsValid)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Re-fetch the home view model to repopulate the lists
        var viewModel = await GetHomeViewModel();
        viewModel.NewStudent = student; // Keep the invalid data for user correction
        return View("Index", viewModel);
    }

    // CRUD Actions for Books
    [HttpPost]
    public async Task<ActionResult> CreateBook(Book book)
    {
        if (ModelState.IsValid)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View("Index", await GetHomeViewModel());
    }

    // CRUD Actions for Authors
    [HttpPost]
    public async Task<ActionResult> CreateAuthor(Author author)
    {
        if (ModelState.IsValid)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return RedirectToAction("Maintain");
        }

        return View("Maintain", await GetMaintainViewModel());
    }

    [HttpPost]
    public async Task<ActionResult> EditAuthor(Author author)
    {
        if (ModelState.IsValid)
        {
            _context.Entry(author).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction("Maintain");
        }

        return View("Maintain", await GetMaintainViewModel());
    }

    [HttpPost]
    public async Task<ActionResult> DeleteAuthor(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author != null)
        {
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Maintain");
    }

    // CRUD Actions for Types
    [HttpPost]
    public async Task<ActionResult> CreateType(Type type)
    {
        if (ModelState.IsValid)
        {
            _context.Types.Add(type);
            await _context.SaveChangesAsync();
            return RedirectToAction("Maintain");
        }

        return View("Maintain", await GetMaintainViewModel());
    }

    [HttpPost]
    public async Task<ActionResult> EditType(Type type)
    {
        if (ModelState.IsValid)
        {
            _context.Entry(type).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction("Maintain");
        }

        return View("Maintain", await GetMaintainViewModel());
    }

    [HttpPost]
    public async Task<ActionResult> DeleteType(int id)
    {
        var type = await _context.Types.FindAsync(id);
        if (type != null)
        {
            _context.Types.Remove(type);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Maintain");
    }

    // CRUD Actions for Borrows
    [HttpPost]
    public async Task<ActionResult> CreateBorrow(Borrow borrow)
    {
        if (ModelState.IsValid)
        {
            _context.Borrows.Add(borrow);
            await _context.SaveChangesAsync();
            return RedirectToAction("Maintain");
        }

        return View("Maintain", await GetMaintainViewModel());
    }

    [HttpPost]
    public async Task<ActionResult> EditBorrow(Borrow borrow)
    {
        if (ModelState.IsValid)
        {
            _context.Entry(borrow).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction("Maintain");
        }

        return View("Maintain", await GetMaintainViewModel());
    }

    [HttpPost]
    public async Task<ActionResult> DeleteBorrow(int id)
    {
        var borrow = await _context.Borrows.FindAsync(id);
        if (borrow != null)
        {
            _context.Borrows.Remove(borrow);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Maintain");
    }

    // Helper method to get the Home view model for Index
    private async Task<LibraryHomeViewModel> GetHomeViewModel()
    {
        int pageSize = 10; // Adjust as needed

        var students = await _context.Students.OrderBy(s => s.Name).ToListAsync();
        var pagedStudents = students.ToPagedList(1, pageSize);

        var books = await _context.Books.ToListAsync();
        var bookStatus = books.Select(b => new BookStatusViewModel
        {
            Book = b,
            Status = _context.Borrows.Any(br => br.BookId == b.BookId) ? "Out" : "Available"
        })
        .OrderBy(b => b.Book.Name)
        .ToList();

        var pagedBooks = bookStatus.ToPagedList(1, pageSize);

        return new LibraryHomeViewModel
        {
            Students = pagedStudents,
            BookStatus = pagedBooks,
            NewStudent = new Student(),
            NewBook = new Book()
        };
    }

    // Helper method to get the Maintain view model for Authors, Types, and Borrows
    private async Task<AdminViewModel> GetMaintainViewModel(int pageAuthors = 1, int pageTypes = 1, int pageBorrows = 1)
    {
        int pageSize = 10; // Adjust the page size as needed

        var authors = (await _context.Authors.OrderBy(a => a.Name).ToListAsync()).ToPagedList(pageAuthors, pageSize);
        var types = (await _context.Types.OrderBy(t => t.Name).ToListAsync()).ToPagedList(pageTypes, pageSize);
        var borrows = (await _context.Borrows
            .Include(b => b.Book)
            .Include(b => b.Student)
            .OrderBy(b => b.BorrowId)
            .ToListAsync())
            .ToPagedList(pageBorrows, pageSize);

        return new AdminViewModel
        {
            Authors = authors,
            Types = types,
            Borrows = borrows
        };
    }

    public async Task<ActionResult> Maintain(int pageAuthors = 1, int pageTypes = 1, int pageBorrows = 1)
    {
        int pageSize = 10; // Number of items per page

        // Get paginated Authors
        var authors = await _context.Authors.OrderBy(a => a.Name).ToListAsync();
        var pagedAuthors = authors.ToPagedList(pageAuthors, pageSize);

        // Get paginated Types
        var types = await _context.Types.OrderBy(t => t.Name).ToListAsync();
        var pagedTypes = types.ToPagedList(pageTypes, pageSize);

        // Get paginated Borrows with necessary includes for related data
        var borrows = await _context.Borrows
            .Include(b => b.Book)
            .Include(b => b.Student)
            .OrderBy(b => b.BorrowId)
            .ToListAsync();
        var pagedBorrows = borrows.ToPagedList(pageBorrows, pageSize);

        var viewModel = new AdminViewModel
        {
            Authors = pagedAuthors,
            Types = pagedTypes,
            Borrows = pagedBorrows,
            NewAuthor = new Author(),
            NewType = new Type(),
            NewBorrow = new Borrow()
        };

        return View(viewModel);
    }


}
