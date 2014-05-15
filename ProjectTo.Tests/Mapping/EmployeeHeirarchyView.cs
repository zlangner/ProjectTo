using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectTo.Tests.Mapping
{
    class EmployeeHeirarchyView
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DepartmentView Dept { get; set; }
    }
}
