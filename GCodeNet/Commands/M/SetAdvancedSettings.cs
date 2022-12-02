using System.Diagnostics;
using System.Text;

namespace GCodeNet.Commands.M
{
	[Command(CommandType.M, 205)]
	public class SetAdvancedSettings : CommandMapping
	{
		//https://marlinfw.org/docs/gcode/M205.html

		[ParameterType("B")]
		public int? MinimumSegmentTime { get; set; }

		[ParameterType("E")]
		public int? EMaxJerk { get; set; }

		[ParameterType("J")]
		public int? JunctionDeviation{ get; set; }

		[ParameterType("S")]
		public int? MinimumFeedrateForPrintMoves { get; set; }

		[ParameterType("T")]
		public int? MinimumFeedrateForTravelMoves { get; set; }

		[ParameterType("X")]
		public int? XMaxJerk { get; set; }

		[ParameterType("Y")]
		public int? YMaxJerk { get; set; }

		[ParameterType("Z")]
		public int? ZMaxJerk { get; set; }

	}
}
