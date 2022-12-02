using System.Text;

namespace GCodeNet.Commands.M
{
	[Command(CommandType.M, 221)]
	public class SetFlowratePercentage : CommandMapping
	{
		[ParameterType("S")]
		public int? FlowratePercentage { get; set; }

		[ParameterType("T")]
		public int? TargetExtruder { get; set; }

	}
}
