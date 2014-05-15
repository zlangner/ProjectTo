using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProjectTo.Tests.Mapping
{
    [TestClass]
    public class ProjectionTests
    {
        IQueryable<Author> CreateAuthors()
        {
            var authors = new []
                {
                    new Author
                        {
                            Id = 1,
                            LastName = "Bradbury",
                            FirstName = "Ray",
                            Country = new Country
                                {
                                    Id = 1,
                                    Name = "USA",
                                    Continent = new Continent
                                        {
                                            Id = 1,
                                            Name = "North America"
                                        }
                                },
                            Books = new List<Book>
                                {
                                    new Book
                                        {
                                            Id = 1,
                                            Title = "Fahrenheit 451"
                                        }
                                }
                        }
                }.AsQueryable();
            return authors;
        }

        IQueryable<EMPLOYEE> CreateEmployees()
        {
            var employees = new[]
                {
                    new EMPLOYEE
                        {
                            ID = 1,
                            LAST_NAME = "Smith",
                            FIRST_NAME = "John",
                            DEPARTMENT_ID = 1,
                            DEPARTMENT = new DEPARTMENT
                                {
                                    ID = 1,
                                    NAME = "IT",
                                    CHIEF_ID = 2,
                                    CHIEF = new EMPLOYEE
                                        {
                                            ID = 2,
                                            LAST_NAME = "Brown",
                                            FIRST_NAME = "Alice"
                                        }
                                }
                        }
                }.AsQueryable();
            return employees;
        }
        
        [TestMethod]
        public void Project_SimpleProperties_OK()
        {
            var authors = CreateAuthors();
            var result = authors.Project().To<AuthorView>().First();
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Bradbury", result.LastName);
            Assert.AreEqual("Ray", result.FirstName);
        }

        [TestMethod]
        public void Project_SimplePropertiesUnderscore_OK()
        {
            var employees = CreateEmployees();
            var result = employees.Project().To<EMPLOYEE_VIEW>().First();
            Assert.AreEqual(1, result.ID);
            Assert.AreEqual("Smith", result.LAST_NAME);
            Assert.AreEqual("John", result.FIRST_NAME);
        }

        /// <summary>
        /// Nested property = CamelCase convention
        /// </summary>
        [TestMethod]
        public void Project_NestedPropertiesCamelCase_OK()
        {
            var authors = CreateAuthors();
            var result = authors.Project().To<AuthorView>().First();
            Assert.AreEqual("USA", result.CountryName);
        }

        /// <summary>
        /// Nested property = Underscore convention
        /// </summary>
        [TestMethod]
        public void Project_NestedPropertiesUnderscore_OK()
        {
            var employees = CreateEmployees();
            var result = employees.Project().To<EMPLOYEE_VIEW>().First();
            Assert.AreEqual("IT", result.DEPARTMENT_NAME);
        }

        /// <summary>
        /// Nested property = CamelCase convention
        /// </summary>
        [TestMethod]
        public void Project_NestedNestedPropertiesCamelCase_OK()
        {
            var authors = CreateAuthors();
            var result = authors.Project().To<AuthorView>().First();
            Assert.AreEqual("North America", result.CountryContinentName);
        }

        /// <summary>
        /// Nested property = Underscore convention
        /// </summary>
        [TestMethod]
        public void Project_NestedNestedPropertiesUnderscore_OK()
        {
            var employees = CreateEmployees();
            var result = employees.Project().To<EMPLOYEE_VIEW>().First();
            Assert.AreEqual("Brown", result.DEPARTMENT_CHIEF_LAST_NAME);
        }

        /// <summary>
        /// Custom mapping
        /// </summary>
        [TestMethod]
        public void Project_CustomMapping_OK()
        {
            var authors = CreateAuthors();
            var result = authors.Project().To<AuthorView>(m=>m.Map(t=>t.BookCount, s=>s.Books.Count())).First();
            Assert.AreEqual(1, result.BookCount);
            Assert.AreEqual("Bradbury", result.LastName); //Other properties are OK too
        }

		/// <summary>
		/// More complex custom mapping
		/// </summary>
		[TestMethod]
		public void Project_CustomMappingNested_OK()
		{
			var authors = CreateAuthors();
			var result = authors.Project().To<AuthorView>(m => m.Map(t => t.BookCount, s => s.Books.Count(b=>b.Title.Length > 0))).First();
			Assert.AreEqual(1, result.BookCount);
			Assert.AreEqual("Bradbury", result.LastName); //Other properties are OK too
		}

		/// <summary>
		/// Ignore default mapping
		/// </summary>
		[TestMethod]
		public void Project_IgnoreDefaultMapping_OK()
		{
			var authors = CreateAuthors();
			var result = authors.Project().To<AuthorView>(m => m.IgnoreDefaultMapping()).First();
			Assert.AreEqual(default(int), result.Id);
			Assert.AreEqual(default(string), result.LastName);
		}
    }
}
