using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;

namespace ProjectTo.Tests.Mapping
{
	[TestClass]
	public class ProjectionBuilderTests
	{
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
		public void ProjectionBuilder_PartialProjection_OK()
		{
			Expression<Func<EMPLOYEE, EmployeeHeirarchyView>> proj = e => new EmployeeHeirarchyView
			{
				Id = e.ID
			};

			var employees = CreateEmployees();

			var result = employees.Select(proj).First();

			//var authors = CreateAuthors();
			//var result = authors.Project().To<AuthorView>().First();
			Assert.AreEqual(1, result.Id);
			Assert.IsNull(result.Dept);
		}

		[TestMethod]
		public void ProjectionBuilder_MergingProjections_OK()
		{
			Expression<Func<EMPLOYEE, EmployeeHeirarchyView>> proj = e => new EmployeeHeirarchyView
			{
				Id = e.ID
			};

			proj = proj.Map(e => new EmployeeHeirarchyView
			{
				Dept = new DepartmentView
				{
					LastName = e.DEPARTMENT.CHIEF.LAST_NAME,
					Name = e.DEPARTMENT.NAME
				}
			});

			var employees = CreateEmployees();

			var result = employees.Select(proj).First();

			//var authors = CreateAuthors();
			//var result = authors.Project().To<AuthorView>().First();
			Assert.AreEqual(1, result.Id);
			Assert.IsNotNull(result.Dept);
			Assert.AreEqual("IT", result.Dept.Name);
			Assert.AreEqual("Brown", result.Dept.LastName);
		}


		[TestMethod]
		public void ProjectionBuilder_MergingProjections2_OK()
		{
			Expression<Func<EMPLOYEE, EmployeeHeirarchyView>> proj = e => new EmployeeHeirarchyView
			{
				Id = e.ID
			};

			proj = proj.Map(e => e.Dept, a => new DepartmentView
			{
				LastName = a.DEPARTMENT.CHIEF.LAST_NAME,
				Name = a.DEPARTMENT.NAME
			});

			var employees = CreateEmployees();

			var result = employees.Select(proj).First();

			//var authors = CreateAuthors();
			//var result = authors.Project().To<AuthorView>().First();
			Assert.AreEqual(1, result.Id);
			Assert.IsNotNull(result.Dept);
			Assert.AreEqual("IT", result.Dept.Name);
			Assert.AreEqual("Brown", result.Dept.LastName);
		}

		[TestMethod]
		public void ProjectionBuilder_NoChaching_OK() 
		{
			var employees = CreateEmployees();
			var proj1Result = employees.Select(ProjectionCacheTest(true)).First();
			var proj2Result = employees.Select(ProjectionCacheTest(false)).First();

			Assert.AreNotEqual( proj1Result.Dept.Name, proj2Result.Dept.Name );
		}

		internal static Expression<Func<EMPLOYEE, EmployeeHeirarchyView>> ProjectionCacheTest( bool changeDeptName ) 
		{
			// To reproduce issue where due to caching we are ignoring changes in the mapping just call ProjectionBuilder constructor with useCache:=true
			var pb = new ProjectionBuilder<EMPLOYEE>(false);

			return pb.ToExpression<EmployeeHeirarchyView>( m => {
				m.IgnoreDefaultMapping();

				m.Map( e => new EmployeeHeirarchyView 
				{
					Id = e.ID
				} );
				m.Map( e => e.Dept, a => new DepartmentView 
				{
					LastName = a.DEPARTMENT.CHIEF.LAST_NAME,
					Name = (changeDeptName ? "Name: " : string.Empty) + a.DEPARTMENT.NAME
				} );
			} );
		}
	}
}
