using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodePlex.LinqProjector.Tests.Mapping
{
    class EMPLOYEE
    {
        public EMPLOYEE()
        {
            this.DEPARTMENT = new DEPARTMENT();
        }
        public int ID { get; set; }
        public string LAST_NAME { get; set; }
        public string FIRST_NAME { get; set; }
        public int DEPARTMENT_ID { get; set; }
        public DEPARTMENT DEPARTMENT { get; set; }
    }
}
