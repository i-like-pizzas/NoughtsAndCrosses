
namespace Domain.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentPlayer">The player going first.</param>
    public class Game(Player currentPlayer) : IGame
    {
        internal Dictionary<byte, Player> board = new Dictionary<byte, Player>();

        public Player CurrentPlayer { get; private set; } = currentPlayer;
        public GameStatus GameStatus { get; private set; } = GameStatus.InProgress;

        public Dictionary<byte, Player> Board
        {
            get { return board; }
            internal set { board = value; }
        }

        private bool IsDraw()
        {
            return !IsWin() && board.Count == 9;
        }

        /// <summary>
        /// Returns true if the current player has won.
        /// </summary>
        /// <returns></returns>
        private bool IsWin()
        {
            var positions = board.Where(x => x.Value == CurrentPlayer).Select(x => x.Key).ToList();

            var winningPositions = new List<byte[]>
            {
                ([ 1, 2, 3 ]),
                ([ 4, 5, 6 ]),
                ([ 7, 8, 9 ]),
                ([ 1, 4, 7 ]),
                ([ 2, 5, 8 ]),
                ([ 3, 6, 9 ]),
                ([ 1, 5, 9 ]),
                ([ 3, 5, 7 ])
            };

            return winningPositions.Any(w => w.Intersect(positions).Count() == 3);
        }

        public PlayResult Play(PlayRequest request)
        {
            if (GameStatus != GameStatus.InProgress)
                return new PlayResult(PlayResultType.GameIsOver, GameStatus);
            else if (CurrentPlayer != request.Player)
                return new PlayResult(PlayResultType.WrongPlayer, GameStatus);
            else if(board.ContainsKey(request.position))
                return new PlayResult(PlayResultType.InvalidPosition, GameStatus);

            board.Add(request.Position, request.Player);

            if(IsWin())
            {
                GameStatus = CurrentPlayer == Player.Crosses ? GameStatus.CrossesWins : GameStatus.NoughtsWins;
            }
            else if(IsDraw())
            {
                GameStatus = GameStatus.Draw;
            }
            else
            {
                CurrentPlayer = CurrentPlayer == Player.Crosses ? Player.Noughts : Player.Crosses;
            }

            return new PlayResult(PlayResultType.Success, GameStatus);
        }
    }
}
