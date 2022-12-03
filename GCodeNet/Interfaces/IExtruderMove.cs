using System;
using System.Collections.Generic;
using System.Text;

namespace GCodeNet.Interfaces
{
	public interface IExtruderMove
	{
		decimal? Extrude { get; set; }
	}
}
