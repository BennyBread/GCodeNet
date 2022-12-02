namespace GCodeNet.Commands
{
    [Command(CommandType.M, 84)]
    public class StopIdleHold : CommandMapping
    {
        [ParameterType("I")]
        public int? ResetFlags { get; set; }

		[ParameterType("E")]
		public bool? EDisable { get; set; }

		[ParameterType("S")]
		public int? InactivityTimeout { get; set; }

		[ParameterType("X")]
		public bool? XDisable { get; set; }

		[ParameterType("Y")]
		public bool? YDisable { get; set; }

		[ParameterType("Z")]
		public bool? ZDisable { get; set; }

	}
}