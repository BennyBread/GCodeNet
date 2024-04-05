using System;
using NUnit.Framework;
using GCodeNet;
using GCodeNet.Commands;

namespace TestProject
{
    [TestFixture]
    public class GCodeFileTest
    {
        [Test]
        public void MultipleCommands()
        {
            var gcode = "G0X1\nG0X2\nG0X3";
            var file = new GCodeFile(gcode);
            Assert.That((decimal)file.Commands[0].GetParameterValue(ParameterType.X) == 1);
            Assert.That((decimal)file.Commands[1].GetParameterValue(ParameterType.X) == 2);
            Assert.That((decimal)file.Commands[2].GetParameterValue(ParameterType.X) == 3);
        }

        [Test]
        public void SingleCommand()
        {
            var gcode = "G0";
            var file = new GCodeFile(gcode);
            Assert.That(((CommandBase)file.Commands[0]).CommandType == CommandType.G);
        }

        [Test]
        public void SingleCommandWithNewLine()
        {
            var gcode = "G0" + Environment.NewLine;
            var file = new GCodeFile(gcode);
            Assert.That(((CommandBase)file.Commands[0]).CommandType == CommandType.G);
        }

        [Test]
        public void MultipleCommandsOnSameLine()
        {
            var gcode = "G0X1G0X2G0X3";
            var file = new GCodeFile(gcode);
            Assert.That((decimal)file.Commands[0].GetParameterValue(ParameterType.X) == 1);
            Assert.That((decimal)file.Commands[1].GetParameterValue(ParameterType.X) == 2);
            Assert.That((decimal)file.Commands[2].GetParameterValue(ParameterType.X) == 3);
        }

        [Test]
        public void CommandsSplitUpOnDifferentLines()
        {
            var gcode = "G0\nX1G0\nX2\nG0\nX\n3";
            var file = new GCodeFile(gcode);
            Assert.That((decimal)file.Commands[0].GetParameterValue(ParameterType.X) == 1);
            Assert.That((decimal)file.Commands[1].GetParameterValue(ParameterType.X) == 2);
            Assert.That((decimal)file.Commands[2].GetParameterValue(ParameterType.X) == 3);
        }

        [Test]
        public void CommandsWithComments()
        {
            var gcode = "G0X1;G1X2\nG1X3";
            var file = new GCodeFile(gcode);
            Assert.That((decimal)file.Commands[0].GetParameterValue(ParameterType.X) == 1);
            Assert.That((decimal)file.Commands[1].GetParameterValue(ParameterType.X) == 3);
        }

        [Test]
        public void CommentsAtStartOfLine()
        {
            var gcode = ";comment\nG1X1\n;comment";
            var file = new GCodeFile(gcode);
            Assert.That((decimal)file.Commands[0].GetParameterValue(ParameterType.X) == 1);
        }

        [Test]
        public void CrcOnly()
        {
            var file = new GCodeFile("*0");
            Assert.That(file.Commands.Count == 0);
        }

        [Test]
        public void InvalidCrcOnly()
        {
            Assert.Catch(typeof(Exception), () => { var file = new GCodeFile("*2"); });
        }

        [Test]
        public void CrcMultipleLines()
        {
            var file = new GCodeFile("G1*118\nG2*117");
            Assert.That(file.Commands[0].CommandSubType == 1);
            Assert.That(file.Commands[1].CommandSubType == 2);
        }

        [Test]
        public void CrcAfterComment()
        {
            var file = new GCodeFile("G1;*118");
            Assert.That(file.Commands[0].CommandSubType == 1);
        }

        [Test]
        public void CommentAfterCrc()
        {
            var file = new GCodeFile("G1*118;comment");
            Assert.That(file.Commands[0].CommandSubType == 1);
        }

        [Test]
        public void EmptyFile()
        {
            var file = new GCodeFile("");
            Assert.That(file.Commands.Count == 0);
        }

        [Test]
        public void TwoCrcException()
        {
            Assert.Catch(typeof(Exception), () => { var file = new GCodeFile("G1*118*118"); });
        }

        [Test]
        public void TwoCommentsSameLine()
        {
            var file = new GCodeFile("G1;comment;comment");
            Assert.That(file.Commands[0].CommandSubType == 1);
        }

        [Test]
        public void IgnoreCrcCheck()
        {
            var options = new GCodeFileOptions();
            options.CheckCRC = false;
            var file = new GCodeFile("G1*0", options);
            Assert.That(file.Commands[0].CommandSubType == 1);
        }

        [Test]
        public void LineNumbersInOrder()
        {
            var file = new GCodeFile("N1N2N3");
            Assert.That(file.Commands.Count == 3);
        }

        [Test]
        public void LineNumbersInOrder2()
        {
            var file = new GCodeFile("N78N79N80");
            Assert.That(file.Commands.Count == 3);
        }

        [Test]
        public void InvalidOrder()
        {
            Assert.Catch(typeof(Exception), () => { var file = new GCodeFile("N2N1N3"); });
        }

        [Test]
        public void IgnoreInvalidOrder()
        {
            var options = new GCodeFileOptions();
            options.CheckLineNumers = false;
            var file = new GCodeFile("N2N1N3", options);
            Assert.That(file.Commands.Count == 3);
        }

        [Test]
        public void UseMappedObjects()
        {
            var options = new GCodeFileOptions();
            options.UseMappedObjects = true;
            var file = new GCodeFile("G1X1", options);
            Assert.That(file.Commands[0] is RapidLinearMove);
        }

        [Test]
        public void NotUseMappedObjects()
        {
            var options = new GCodeFileOptions();
            options.UseMappedObjects = false;
            var file = new GCodeFile("G1X1", options);
            Assert.That(!(file.Commands[0] is RapidLinearMove));
        }

        [Test]
        public void ExportWithNoLineNumbersOrCrc()
        {
            var options = new ExportFileOptions();
            options.WriteCRC = false;
            options.WriteLineNumbers = false;
            var file = new GCodeFile("G1X1G1X2");
            var gCode = file.ToGCode(options);
            Assert.That(gCode == "G1 X1 S0" + Environment.NewLine + "G1 X2 S0" + Environment.NewLine);
        }

        [Test]
        public void ExportWithLineNumbersOnly()
        {
            var options = new ExportFileOptions();
            options.WriteCRC = false;
            options.WriteLineNumbers = true;
            var file = new GCodeFile("G1X1G1X2");
            Assert.That(file.ToGCode(options) == "N1 G1 X1 S0" + Environment.NewLine + "N2 G1 X2 S0" + Environment.NewLine);
        }

        [Test]
        public void ExportWithCrcOnly()
        {
            var options = new ExportFileOptions();
            options.WriteCRC = true;
            options.WriteLineNumbers = false;
            var file = new GCodeFile("G1X1G1X2");
            Assert.That(file.ToGCode(options) == "G1 X1 S0*124" + Environment.NewLine + "G1 X2 S0*127" + Environment.NewLine);
        }

        [Test]
        public void ExportWithLineNumbersAndCrc()
        {
            var options = new ExportFileOptions();
            options.WriteCRC = true;
            options.WriteLineNumbers = true;
            var file = new GCodeFile("G1X1G1X2");
            Assert.That(file.ToGCode(options) == "N1 G1 X1 S0*35" + Environment.NewLine + "N2 G1 X2 S0*35" + Environment.NewLine);
        }

        [Test]
        public void RemoveOldLineNumbers()
        {
            var options = new ExportFileOptions();
            options.WriteCRC = false;
            options.WriteLineNumbers = true;
            var file = new GCodeFile("N5 G1X1 N6 G1X2");
            Assert.That(file.ToGCode(options) == "N1 G1 X1 S0" + Environment.NewLine + "N2 G1 X2 S0" + Environment.NewLine);
        }
    }
}
