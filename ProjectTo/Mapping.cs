using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ProjectTo
{
    class Mapping
    {
        public PropertyInfo DestPropertyInfo { get; private set; }
        public LambdaExpression Transform { get; private set; }

        public Mapping(PropertyInfo propertyInfo, LambdaExpression transform)
        {
            DestPropertyInfo = propertyInfo;
            Transform = transform;
        }
    }
}
