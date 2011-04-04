using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Data;

namespace Simple.Data.Generation.Tests.Model
{
    public class Users
    {
        private dynamic db = Database.Open();

        public dynamic CreateSomeViewModel()
        {
            return db.Users.FindByUsername("bob");
        }

        public dynamic CreateAnotherViewModel()
        {
            return db.Users.FindBySomeId(4);
        }
    }
}
