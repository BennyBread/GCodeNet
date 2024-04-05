﻿using NUnit.Framework;
using GCodeNet;

namespace TestProject
{
    [Command(CommandType.M,999)]
    internal class IntClass : CommandMapping
    {
        [ParameterType(ParameterType.X)]
        public int X { get; set; }
        [ParameterType(ParameterType.Y)]
        public int? Y { get; set; }
    }

    [TestFixture]
    public class IntType
    {
        [Test]
        public void TestInt()
        {
            CommandReflection.AddMappedType(typeof(IntClass));
            var c1 = (IntClass)CommandMapping.Parse("M999 X-1.1");
            Assert.That(c1.X == -1);
            Assert.That(c1.ToGCode() == "M999 X-1");
            var c2 = (IntClass)CommandMapping.Parse("M999 X1.1");
            Assert.That(c2.X == 1);
            Assert.That(c2.ToGCode() == "M999 X1");
            var c3 = (IntClass)CommandMapping.Parse("M999 X1");
            Assert.That(c3.X == 1);
            Assert.That(c3.ToGCode() == "M999 X1");
            var c4 = (IntClass)CommandMapping.Parse("M999 X");
            Assert.That(c4.X == 0);
            Assert.That(c4.ToGCode() == "M999 X0");
            var c5 = (IntClass)CommandMapping.Parse("M999");
            Assert.That(c5.X == 0);
            Assert.That(c5.ToGCode() == "M999 X0");
        }

        [Test]
        public void TestNullableInt()
        {
            CommandReflection.AddMappedType(typeof(IntClass));
            var c1 = (IntClass)CommandMapping.Parse("M999 Y-1.1");
            Assert.That(c1.Y == -1);
            Assert.That(c1.ToGCode() == "M999 X0 Y-1");
            var c2 = (IntClass)CommandMapping.Parse("M999 Y1.1");
            Assert.That(c2.Y == 1);
            Assert.That(c2.ToGCode() == "M999 X0 Y1");
            var c3 = (IntClass)CommandMapping.Parse("M999 Y1");
            Assert.That(c3.Y == 1);
            Assert.That(c3.ToGCode() == "M999 X0 Y1");
            var c4 = (IntClass)CommandMapping.Parse("M999 Y");
            Assert.That(c4.Y == null);
            Assert.That(c4.ToGCode() == "M999 X0");
            var c5 = (IntClass)CommandMapping.Parse("M999");
            Assert.That(c5.Y == null);
            Assert.That(c5.ToGCode() == "M999 X0");
        }
    }
}
