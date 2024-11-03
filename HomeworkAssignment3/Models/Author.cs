using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeworkAssignment3.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public virtual ICollection<Book> Books { get; set; }



    }

}