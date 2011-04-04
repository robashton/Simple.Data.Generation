using NUnit.Framework;
using Simple.Data.Generation.Tests.Model;

namespace Simple.Data.Generation.Tests
{
    [TestFixture]
    public class FeatureTests
    {
        [Test]
        public void Simple_Query_Against_Table_Results_In_String_Property_With_Constant_Being_Discovered()
        {
            var scanner = new ModelScanner();
            var model = scanner.CreateModelFromAssembly<Users>();
            var table = model.Table("Users");
            var property = table.Column("Username");
            Assert.That(property.Type, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void Simple_Query_Against_Table_Results_In_Int_Property_With_Constant_Being_Discovered()
        {
            var scanner = new ModelScanner();
            var model = scanner.CreateModelFromAssembly<Users>();
            var table = model.Table("Users");
            var property = table.Column("SomeId");
            Assert.That(property.Type, Is.EqualTo(typeof(int)));
        }
    }
}