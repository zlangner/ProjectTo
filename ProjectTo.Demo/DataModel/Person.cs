using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectTo.Demo.DataModel
{
    public class Person
    {
	    public Person()
	    {
		    this.Books = new List<Book>();
	    }
		public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }
		public virtual ICollection<Book> Books { get; set; }
    }
}
