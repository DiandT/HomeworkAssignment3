using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Optimization;
using System.Web.Razor.Parser.SyntaxTree;
using System.Web.UI.WebControls;

namespace HomeworkAssignment3.Models
{
    public class Type
    {
        public int TypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }

}
