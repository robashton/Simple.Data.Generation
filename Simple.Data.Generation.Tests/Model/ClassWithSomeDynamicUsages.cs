using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Data.Generation.Tests.Model
{
    public class ClassWithSomeDynamicUsages
    {
        private dynamic db = Database.Open();

        public dynamic MethodWithSingleDynamicCall()
        {
            return db.Users;
        }
    }
}
