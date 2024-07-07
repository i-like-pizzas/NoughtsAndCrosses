namespace Domain.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="player">The player making the move.</param>
    /// <param name="position">The position to mark on the board (1-9).</param>
    public record PlayRequest(Player player, byte position)
    {
        public Player Player { get; private set; } = player;

        public byte Position { get; private set; } = position;
    }
}
