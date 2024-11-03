using PagedList;
using System.Collections.Generic;

namespace HomeworkAssignment3.Models
{
    using PagedList;

    public class AdminViewModel
    {
        public IPagedList<Author> Authors { get; set; }
        public IPagedList<Type> Types { get; set; }
        public IPagedList<Borrow> Borrows { get; set; }

        public Author NewAuthor { get; set; }
        public Type NewType { get; set; }
        public Borrow NewBorrow { get; set; }
    }



}
