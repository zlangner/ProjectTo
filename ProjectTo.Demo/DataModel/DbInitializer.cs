using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace ProjectTo.Demo.DataModel
{
    class DbInitializer : CreateDatabaseIfNotExists<DemoDbContext>
    {
        protected override void Seed(DemoDbContext context)
        {
            var Russia = new Country
                {
                    Name = "Russia"
                };
			var USA = new Country
				{
					Name = "United States"
				};
            context.Countries.Add(Russia);
	        context.Countries.Add(USA);
            context.People.Add(new Person
                {
                    LastName = "Tolstoy",
                    FirstName = "Leo",
                    Country = Russia,
					Books = new []
						{
							new Book {Title = "War And Peace", Price = 100}
						}
                });
            
            context.SaveChanges();
            
            base.Seed(context);
        }
    }
}
