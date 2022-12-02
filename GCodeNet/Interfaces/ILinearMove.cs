using System;
using System.Collections.Generic;
using System.Text;

namespace GCodeNet.Interfaces
{
	public interface ILinearMove
	{
		decimal? MoveX { get; set; }

		decimal? MoveY { get; set; }

		decimal? MoveZ { get; set; }
	}
}
