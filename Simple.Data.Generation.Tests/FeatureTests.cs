using NUnit.Framework;
using Simple.Data.Generation.Tests.Model;

namespace Simple.Data.Generation.Tests
{
    [TestFixture]
    public class FeatureTests
    {
        [Test]
        public void Simple_Query_Against_Table_Results_In_String_Property_Being_Discovered()
        {
            var scanner = new ModelScanner();
            var model = scanner.CreateModelFromAssembly<Users>();
            var table = model.GetTable("Users");
            var property = table.GetColumn("Username");
            Assert.That(property.Type, Is.EqualTo(typeof(string)));
        }
    }
}