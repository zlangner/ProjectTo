using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodePlex.LinqProjector.Tests.Mapping
{
    class Country
    {
        public Country()
        {
            this.Continent = new Continent();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public Continent Continent { get; set; }
    }
}
