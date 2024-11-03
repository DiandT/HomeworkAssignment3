using PagedList;
using System.Collections.Generic;

namespace HomeworkAssignment3.Models
{
    public class LibraryHomeViewModel
    {
        public IPagedList<Student> Students { get; set; }

        public IPagedList<Book> Books { get; set; }

        public IPagedList<BookStatusViewModel> BookStatus { get; set; }

        public Student NewStudent { get; set; } = new Student();
        public Book NewBook { get; set; } = new Book();
    }

    public class BookStatusViewModel
    {
        public Book Book { get; set; }
        public string Status { get; set; }
    }


}
