using NUnit.Framework;
using GCodeNet;

namespace TestProject
{
    [Command(CommandType.M,999)]
    internal class BoolClass : CommandMapping
    {
        [ParameterType(ParameterType.X)]
        public bool X { get; set; }

    }

    [TestFixture]
    public class BoolType
    {
        [Test]
        public void TestBool()
        {
            CommandReflection.AddMappedType(typeof(BoolClass));
            var c1 = (BoolClass)CommandMapping.Parse("M999 X-1.1");
            Assert.That(c1.X);
            Assert.That(c1.ToGCode() == "M999 X");
            var c2 = (BoolClass)CommandMapping.Parse("M999 X1.1");
            Assert.That(c2.X);
            Assert.That(c2.ToGCode() == "M999 X");
            var c3 = (BoolClass)CommandMapping.Parse("M999 X1");
            Assert.That(c3.X);
            Assert.That(c3.ToGCode() == "M999 X");
            var c4 = (BoolClass)CommandMapping.Parse("M999 X");
            Assert.That(c4.X);
            Assert.That(c4.ToGCode() == "M999 X");
            var c5 = (BoolClass)CommandMapping.Parse("M999");
            Assert.That(c5.X == false);
            Assert.That(c5.ToGCode() == "M999");
        }
    }
}
