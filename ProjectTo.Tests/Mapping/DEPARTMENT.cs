using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectTo.Tests.Mapping
{
    class DEPARTMENT
    {
        public DEPARTMENT()
        {
        }
        public int ID { get; set; }
        public string NAME { get; set; }
        public int CHIEF_ID { get; set; }
        public EMPLOYEE CHIEF { get; set; }
    }
}
