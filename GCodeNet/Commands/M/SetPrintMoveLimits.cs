using System.Text;

namespace GCodeNet.Commands.M
{
	[Command(CommandType.M, 201)]
	public class SetPrintMoveLimits : CommandMapping
	{
		[ParameterType("E")]
		public int? EAxisMaxAcceleration { get; set; }

		[ParameterType("X")]
		public int? XAxisMaxAcceleration { get; set; }

		[ParameterType("Y")]
		public int? YAxisMaxAcceleration { get; set; }

		[ParameterType("Z")]
		public int? ZAxisMaxAcceleration { get; set; }

	}
}
