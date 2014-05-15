using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodePlex.LinqProjector.Tests.Mapping
{
    class Author
    {
        public Author()
        {
            this.Country = new Country();
            this.Books = new List<Book>();
        }
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public Country Country { get; set; }
        public List<Book> Books { get; set; }

    }
}
