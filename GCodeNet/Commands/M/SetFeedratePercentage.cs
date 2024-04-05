namespace GCodeNet.Commands.M
{
	[Command(CommandType.M, 220)]
	public class SetFeedratePercentage : CommandMapping
	{
		[ParameterType("B")]
		public bool? BackupFactor { get; set; }

		[ParameterType("R")]
		public bool? RestoreFactor { get; set; }

		[ParameterType("S")]
		public int? FeedratePercentage { get; set; }

	}
}
