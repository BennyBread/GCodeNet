using GCodeNet.Interfaces;

namespace GCodeNet.Commands
{
    [Command(CommandType.G, 1)]
    public class RapidLinearMove : LinearMove, ILinearMove, IExtruderMove
	{
    }
}