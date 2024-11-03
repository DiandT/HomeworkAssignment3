using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeworkAssignment3.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Name { get; set; }
        public int PageCount { get; set; }
        public int Point { get; set; }
        public int AuthorId { get; set; }
        public int TypeId { get; set; }

        public virtual Author Author { get; set; }
        public virtual Type Type { get; set; }
        public virtual ICollection<Borrow> Borrows { get; set; }
    }

}