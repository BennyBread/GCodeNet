namespace GCodeNet.Interfaces
{
	public interface ILinearMove
	{
		decimal? MoveX { get; set; }

		decimal? MoveY { get; set; }

		decimal? MoveZ { get; set; }
	}
}
