namespace Domain.Models
{
    public record PlayResult(PlayResultType resultType, GameStatus gameStatus)
    {
        public PlayResultType ResultType { get; private set; } = resultType;

        public GameStatus GameStatus { get; private set; } = gameStatus;
    }
}
