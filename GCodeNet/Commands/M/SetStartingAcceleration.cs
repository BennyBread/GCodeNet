using System.Text;

namespace GCodeNet.Commands.M
{
	[Command(CommandType.M, 204)]
	public class SetStartingAcceleration : CommandMapping
	{
		[ParameterType("P")]
		public int? PrintingAcceleration { get; set; }

		[ParameterType("R")]
		public int? RetractAcceleration { get; set; }

		[ParameterType("S")]
		public int? LegacyParameter{ get; set; }

		[ParameterType("T")]
		public int? TravelAcceleration { get; set; }

	}
}
