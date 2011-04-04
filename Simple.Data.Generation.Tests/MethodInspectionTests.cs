using NUnit.Framework;
using Simple.Data.Generation.Tests.Model;

namespace Simple.Data.Generation.Tests
{
    [TestFixture]
    public class MethodInspectionTests
    {
        [Test]
        public void Can_Extract_Dynamic_Method_Invocations_From_A_Method()
        {
            var inspector = new MethodInspectorBuilder()
                .ForClass<ClassWithSomeDynamicUsages>()
                .WithMethod("MethodWithSingleDynamicCall")
                .Get();

            var methodInvocations = inspector.ExtractDynamicMethodCallInstructions();
            Assert.That(methodInvocations, Has.Length.EqualTo(1));
        }

        [Test]
        public void Can_Extract_Dynamic_Cached_Fields_From_A_Method()
        {
            var inspector = new MethodInspectorBuilder()
                .ForClass<ClassWithSomeDynamicUsages>()
                .WithMethod("MethodWithSingleDynamicCall")
                .Get();

            var cachedFields = inspector.ExtractInitialReflectedCachedFieldReferenceInstructions();
            Assert.That(cachedFields, Has.Length.EqualTo(1));
        }
    }
}