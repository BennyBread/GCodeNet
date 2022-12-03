using System.Diagnostics;
using System.Text;

namespace GCodeNet.Commands.M
{
	[Command(CommandType.M, 205)]
	public class SetAdvancedSettings : CommandMapping
	{
		//https://marlinfw.org/docs/gcode/M205.html

		[ParameterType("B")]
		public decimal? MinimumSegmentTime { get; set; }

		[ParameterType("E")]
		public decimal? EMaxJerk { get; set; }

		[ParameterType("J")]
		public decimal? JunctionDeviation{ get; set; }

		[ParameterType("S")]
		public decimal? MinimumFeedrateForPrintMoves { get; set; }

		[ParameterType("T")]
		public decimal? MinimumFeedrateForTravelMoves { get; set; }

		[ParameterType("X")]
		public decimal? XMaxJerk { get; set; }

		[ParameterType("Y")]
		public decimal? YMaxJerk { get; set; }

		[ParameterType("Z")]
		public decimal? ZMaxJerk { get; set; }

	}
}
