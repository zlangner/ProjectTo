using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectTo.Tests.Mapping
{
    class AuthorView
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string CountryName { get; set; }
        public string CountryContinentName { get; set; }
        public int BookCount { get; set; }
    }
}
