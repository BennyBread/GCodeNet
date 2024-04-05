using System;
using NUnit.Framework;
using GCodeNet;
using GCodeNet.Commands;
using System.Linq;

namespace TestProject
{
    internal class MissingAttributeClass : CommandMapping
    {
    }

    [Command(CommandType.M, 999)]
    internal class CustomCommand : CommandMapping
    {
        [ParameterType(ParameterType.X)]
        public int X { get; set; }
    }

    [TestFixture]
    public class MappedCommand
    {
        [Test]
        public void RapidLinearMoveTest()
        {
            var cmd = new RapidLinearMove();
            cmd.MoveX = 1;
            cmd.MoveY = 1.2m;

            Assert.That(cmd.CommandType == CommandType.G);
            Assert.That(cmd.CommandSubType == 1);

            var parameters = cmd.GetParameters().ToArray();
            Assert.That(parameters[0] == ParameterType.X);
            Assert.That(parameters[1] == ParameterType.Y);
            Assert.That(parameters[2] == ParameterType.S);

            Assert.That((decimal)cmd.GetParameterValue(ParameterType.X) == 1);
            Assert.That((decimal)cmd.GetParameterValue(ParameterType.Y) == 1.2m);
            Assert.That((decimal)cmd.GetParameterValue(ParameterType.S) == 0);

            Assert.That(cmd.ToGCode() == "G1 X1 Y1.2 S0");
        }

        [Test]
        public void SetExtruderTemperatureTest()
        {
            var cmd = new SetExtruderTemperature();
            cmd.Temperature = 98;

            Assert.That(cmd.CommandType == CommandType.M);
            Assert.That(cmd.CommandSubType == 104);

            var parameters = cmd.GetParameters().ToArray();
            Assert.That(parameters[0] == ParameterType.S);
            Assert.That((decimal)cmd.GetParameterValue(ParameterType.S) == 98);

            Assert.That(cmd.ToGCode() == "M104 S98");
        }

        [Test]
        public void MissingCommandAttribute()
        {
            Assert.Catch(typeof(Exception), () => { var cmd = new MissingAttributeClass(); });
        }

        [Test]
        public void MappingPropertyTest()
        {
            var cmd = new SetExtruderTemperature();
            cmd.Temperature = 98;
            Assert.That((decimal)cmd.GetParameterValue(ParameterType.S) == 98);

            cmd.SetParameterValue(ParameterType.S, 33);
            Assert.That(cmd.Temperature == 33);

            cmd.SetParameterValue(ParameterType.S, null);
            Assert.That(cmd.Temperature == null);

            cmd.SetParameterValue(ParameterType.S, 1);
            cmd.Temperature = null;
            Assert.That(cmd.GetParameterValue(ParameterType.S) == null);

        }


        [Test]
        public void AutoAddCustomCommand()
        {
            CommandReflection.ClearMappings();
            var cmd = new CustomCommand();
            var g = cmd.ToGCode();
        }

        [Test]
        public void CustomCommandMappingException()
        {
            CommandReflection.ClearMappings();
            Assert.Catch(typeof(Exception), () => { var cmd = CommandMapping.Parse("M999 X"); });
        }

        [Test]
        public void CustomCommandAddType()
        {
            CommandReflection.ClearMappings();
            CommandReflection.AddMappedType(typeof(CustomCommand));
            var cmd = CommandMapping.Parse("M999 X");
        }

        [Test]
        public void CustomCommandAddAssembly()
        {
            CommandReflection.ClearMappings();
            CommandReflection.AddMappedTypesFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
            var cmd = CommandMapping.Parse("M999 X");
        }

        [Test]
        public void AutoAddCustomCommand2()
        {
            CommandReflection.ClearMappings();
            var cmd = CommandMapping.Parse(typeof(CustomCommand), "M999 X");
            var g = cmd.ToGCode();
        }
    }
}
