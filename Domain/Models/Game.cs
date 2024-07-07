
namespace Domain.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentPlayer">The player going first.</param>
    public class Game(Player currentPlayer) : IGame
    {
        internal Dictionary<byte, Player?> board = new Dictionary<byte, Player?>();

        public Player CurrentPlayer { get; private set; } = currentPlayer;

        public Dictionary<byte, Player?> Board
        {
            get { return board; }
            internal set { board = value; }
        }

        public GameStatus GameStatus => throw new NotImplementedException();

        public PlayResult Play(PlayRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
