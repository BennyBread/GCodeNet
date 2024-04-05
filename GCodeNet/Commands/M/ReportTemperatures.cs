namespace GCodeNet.Commands.M
{
	[Command(CommandType.M, 105)]
	public class ReportTemperatures : CommandMapping
	{
		[ParameterType("T")]
		public int? HotendIndex { get; set; }

	}
}
