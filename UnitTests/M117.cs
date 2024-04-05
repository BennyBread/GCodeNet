using NUnit.Framework;
using GCodeNet;

namespace TestProject
{
    [TestFixture]
    public class M117Test
    {
        [Test]
        public void M117CommandParse()
        {
            var cmd = CommandBase.Parse("M117 Hello World");
            Assert.That(cmd.CommandType == CommandType.M);
            Assert.That(cmd.CommandSubType == 117);
            Assert.That(cmd.ToGCode() == "M117 Hello World");
        }

        [Test]
        public void M117CommandParseEmpty()
        {
            var cmd = CommandBase.Parse("M117");
            Assert.That(cmd.CommandType == CommandType.M);
            Assert.That(cmd.CommandSubType == 117);
            Assert.That(cmd.ToGCode() == "M117");
        }

        [Test]
        public void M117CommandNoWhiteSpace()
        {
            var cmd = CommandBase.Parse("M117Hello");
            Assert.That(cmd.CommandType == CommandType.M);
            Assert.That(cmd.CommandSubType == 117);
            Assert.That(cmd.ToGCode() == "M117 Hello");
        }

        [Test]
        public void M117CommandExtraWhiteSpace()
        {
            var cmd = CommandBase.Parse("   M117        Hello");
            Assert.That(cmd.CommandType == CommandType.M);
            Assert.That(cmd.CommandSubType == 117);
            Assert.That(cmd.ToGCode() == "M117 Hello");
        }

        [Test]
        public void M117CommandParseMultiple()
        {
            var cmd = CommandBase.Parse("M117 Hello M117 Hello");
            Assert.That(cmd.CommandType == CommandType.M);
            Assert.That(cmd.CommandSubType == 117);
            Assert.That(cmd.ToGCode() == "M117 Hello M117 Hello");
        }

        [Test]
        public void M117CommandParseInvalidChars()
        {
            var cmd = CommandBase.Parse("M117 Hello:?.+-';*");
            Assert.That(cmd.CommandType == CommandType.M);
            Assert.That(cmd.CommandSubType == 117);
            Assert.That(cmd.ToGCode() == "M117 Hello:?.+-';*");
        }

        [Test]
        public void M117FileParserGBeforeM()
        {
            var gcode = "G1M117 Hello";
            var file = new GCodeFile(gcode);
            Assert.That(file.Commands.Count ==2);
            Assert.That(file.Commands[0].CommandType == CommandType.G);
            Assert.That(file.Commands[1].CommandType == CommandType.M);
            Assert.That(file.Commands[1].ToGCode() == "M117 Hello");
        }

        [Test]
        public void M117FileParserGAfterM()
        {
            var gcode = "M117G1 Hello";
            var file = new GCodeFile(gcode);
            Assert.That(file.Commands.Count == 1);
            Assert.That(file.Commands[0].CommandType == CommandType.M);
            Assert.That(file.Commands[0].ToGCode() == "M117 G1 Hello");
        }

        [Test]
        public void M117FileParserMultipleLines()
        {
            var gcode = "M117 Hello\r\nM117 World";
            var file = new GCodeFile(gcode);
            Assert.That(file.Commands.Count == 2);
            Assert.That(file.Commands[0].CommandType == CommandType.M);
            Assert.That(file.Commands[0].ToGCode() == "M117 Hello");
            Assert.That(file.Commands[1].CommandType == CommandType.M);
            Assert.That(file.Commands[1].ToGCode() == "M117 World");
        }

        [Test]
        public void M117FileParserWithComment()
        {
            var gcode = "M117 Hello;World";
            var file = new GCodeFile(gcode);
            Assert.That(file.Commands.Count == 1);
            Assert.That(file.Commands[0].CommandType == CommandType.M);
            Assert.That(file.Commands[0].ToGCode() == "M117 Hello");
        }

        [Test]
        public void M117FileParserWithCRC()
        {
            var gcode = "M117 Hello*24";
            var file = new GCodeFile(gcode);
            Assert.That(file.Commands.Count == 1);
            Assert.That(file.Commands[0].CommandType == CommandType.M);
            Assert.That(file.Commands[0].ToGCode() == "M117 Hello");
        }
    }
}