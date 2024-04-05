namespace GCodeNet.Commands.M
{
	[Command(CommandType.M, 190)]
	public class SetBedTemperatureAndWait : CommandMapping
	{
		[ParameterType("S")]
		public int? HeatingTemperature { get; set; }

		[ParameterType("R")]
		public int? HeatingAndCoolingTemperature { get; set; }

	}
}
