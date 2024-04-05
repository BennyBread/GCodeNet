using NUnit.Framework;
using GCodeNet;

namespace TestProject
{
    [Command(CommandType.M,999)]
    internal class ByteClass : CommandMapping
    {
        [ParameterType(ParameterType.X)]
        public byte X { get; set; }
        [ParameterType(ParameterType.Y)]
        public byte? Y { get; set; }
    }

    [TestFixture]
    public class ByteType
    {
        [Test]
        public void TestByte()
        {
            CommandReflection.AddMappedType(typeof(ByteClass));
            var c2 = (ByteClass)CommandMapping.Parse("M999 X1.1");
            Assert.That(c2.X == 1);
            Assert.That(c2.ToGCode() == "M999 X1");
            var c3 = (ByteClass)CommandMapping.Parse("M999 X1");
            Assert.That(c3.X == 1);
            Assert.That(c3.ToGCode() == "M999 X1");
            var c4 = (ByteClass)CommandMapping.Parse("M999 X");
            Assert.That(c4.X == 0);
            Assert.That(c4.ToGCode() == "M999 X0");
            var c5 = (ByteClass)CommandMapping.Parse("M999");
            Assert.That(c5.X == 0);
            Assert.That(c5.ToGCode() == "M999 X0");
        }

        [Test]
        public void TestNullableByte()
        {
            CommandReflection.AddMappedType(typeof(ByteClass));
            var c2 = (ByteClass)CommandMapping.Parse("M999 Y1.1");
            Assert.That(c2.Y == 1);
            Assert.That(c2.ToGCode() == "M999 X0 Y1");
            var c3 = (ByteClass)CommandMapping.Parse("M999 Y1");
            Assert.That(c3.Y == 1);
            Assert.That(c3.ToGCode() == "M999 X0 Y1");
            var c4 = (ByteClass)CommandMapping.Parse("M999 Y");
            Assert.That(c4.Y == null);
            Assert.That(c4.ToGCode() == "M999 X0");
            var c5 = (ByteClass)CommandMapping.Parse("M999");
            Assert.That(c5.Y == null);
            Assert.That(c5.ToGCode() == "M999 X0");
        }
    }
}
