using System.Text;

namespace GCodeNet.Commands.M
{
	[Command(CommandType.M, 203)]
	public class SetMaxFeedRate : CommandMapping
	{
		[ParameterType("E")]
		public int? EAxisMaxFeedrate { get; set; }

		[ParameterType("X")]
		public int? XAxisMaxFeedrate { get; set; }

		[ParameterType("Y")]
		public int? YAxisMaxFeedrate { get; set; }

		[ParameterType("Z")]
		public int? ZAxisMaxFeedrate { get; set; }

	}
}
