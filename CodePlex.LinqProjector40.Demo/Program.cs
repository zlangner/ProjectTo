using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using CodePlex.LinqProjector;
using CodePlex.LinqProjector40.Demo.DataModel;

namespace CodePlex.LinqProjector40.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DbInitializer());

            using (var context = new DemoDbContext())
            {
                //Simple Demo2
                var firstAuthor = context.People
                    .Project().To<PersonView1>().FirstOrDefault();
                if(firstAuthor != null)
                    Console.WriteLine("{0} from {1}", firstAuthor.LastName, firstAuthor.CountryName);

				//Custom mapping
				if (true)
				{
					var author = context.People
						.Project().To<PersonView2>(m => m.Map(t => t.HasCountry, s => s.CountryId != null))
						.FirstOrDefault();
					if (author != null)
						Console.WriteLine("Has country - {0}", author.HasCountry);
				}

				//Custom mapping (2)
				if (true)
				{
					var author = context.People
						.Project().To<PersonView2>(PersonView2Mapping)
						.FirstOrDefault();
					if (author != null)
						Console.WriteLine("Has country - {0}", author.HasCountry);
				}

				//Custom mapping (3)
				if (true)
				{
					var author = context.People
						.Project().To<PersonView3>(m=>m.Map(t=>t.Sum, s=>s.Books.Sum(b=>b.Price)))
						.FirstOrDefault();
					if (author != null)
						Console.WriteLine("Total price - {0}", author.Sum);
				}
            }
            
            Console.ReadLine();
        }

		static void PersonView2Mapping(Mapper<Person, PersonView2> m)
		{
			m.Map(t => t.HasCountry, s => s.CountryId != null);
		}
    }

	

    internal class PersonView1
    {
        public string LastName { get; set; }
        public string CountryName { get; set; }
    }

	internal class PersonView2
	{
		public string LastName { get; set; }
		public bool HasCountry { get; set; }
	}

	internal class PersonView3
	{
		public string LastName { get; set; }
		public decimal Sum { get; set; }
	}
}
