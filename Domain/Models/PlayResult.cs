namespace Domain.Models
{
    public record PlayResult(PlayResultType resultType, string board, GameStatus gameStatus)
    {
        public PlayResultType ResultType { get; private set; } = resultType;

        public string Board { get; private set; } = board;

        public GameStatus GameStatus { get; private set; } = gameStatus;
    }
}
