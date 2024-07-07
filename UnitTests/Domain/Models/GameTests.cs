using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Domain.Models
{
    public class GameTests
    {
        [Fact]
        public void TheDefaultStatusIsInProgress()
        {
            // ARRANGE
            var game = new Game(Player.Crosses);

            // ACT
            var status = game.GameStatus;

            // ASSERT
            Assert.Equal(GameStatus.InProgress, status);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        public void Play_TheBoardIsUpdatedCorrectly(byte position)
        {
            // ARRANGE
            var game = new Game(Player.Crosses);
            var currentPlayer = game.CurrentPlayer;

            // ACT
            var playResult = game.Play(new PlayRequest(currentPlayer, position));

            // ASSERT
            Assert.True(PlayResultType.Success == playResult.ResultType);
            Assert.True(game.Board[position] == currentPlayer);
        }

        [Fact]
        public void Play_AfterMarkingTheBoardTheNextPlayerIsSelectedIfTheGameIsStillActive()
        {
            // ARRANGE
            var game = new Game(Player.Crosses);
            var currentPlayer = game.CurrentPlayer;

            // ACT
            game.Play(new PlayRequest(currentPlayer, 1));

            // ASSERT
            Assert.Equal(GameStatus.InProgress, game.GameStatus);
            Assert.NotEqual(game.CurrentPlayer, currentPlayer);
        }

        private static Player GetOtherPlayer(Player player)
        {
            return Enum.GetValues<Player>().Cast<Player>().First(x => x != player);
        }

        [Fact]
        public void Play_AfterMarkingTheBoardTheDrawConditionIsSetIfNoMoreMovesArePossibleAndNoPlayerHasWon()
        {
            // ARRANGE
            var game = new Game(Player.Crosses);

            // ACT
            var player1 = game.CurrentPlayer;
            var player2 = GetOtherPlayer(player1);

            game.Play(new PlayRequest(player1, 1));
            game.Play(new PlayRequest(player2, 2));
            game.Play(new PlayRequest(player1, 3));
            game.Play(new PlayRequest(player2, 4));
            game.Play(new PlayRequest(player1, 6));
            game.Play(new PlayRequest(player2, 5));
            game.Play(new PlayRequest(player1, 7));
            game.Play(new PlayRequest(player2, 9));
            
            var lastMove = game.Play(new PlayRequest(player1, 8));

            // ASSERT
            Assert.Equal(GameStatus.Draw, game.GameStatus);
            Assert.True(lastMove.ResultType == PlayResultType.Success);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(4, 5, 6)]
        [InlineData(7, 8, 9)]
        [InlineData(1, 4, 7)]
        [InlineData(2, 5, 8)]
        [InlineData(3, 6, 9)]
        [InlineData(1, 5, 9)]
        [InlineData(3, 5, 7)]
        public void Play_AfterMarkingTheBoardTheWinConditionIsSetIfThePlayerHasALineOfThree(byte move1, byte move2, byte move3)
        {
            foreach (var playerType in Enum.GetValues<Player>().Cast<Player>())
            {
                // ARRANGE
                var game = new Game(playerType);
                var currentPlayer = game.CurrentPlayer;

                game.Board = new Dictionary<byte, Player>
                {
                    { move1, currentPlayer },
                    { move2, currentPlayer }
                };

                // ACT
                var playResult = game.Play(new PlayRequest(currentPlayer, move3));

                // ASSERT
                Assert.Equal(game.Winner, currentPlayer);
                Assert.Equal(GameStatus.Win, game.GameStatus);
                Assert.True(playResult.ResultType == PlayResultType.Success);
            }
        }

        [Fact]
        public void Play_PlayResultTypeIsGameIsOverIfTheGameIsOver()
        {
            // ARRANGE
            var winner = Player.Noughts;
            var game = new Game(winner);

            game.Board = new Dictionary<byte, Player>
            {
                { 1, winner },
                { 2, winner }
            };

            // ACT
            var winningMove = game.Play(new PlayRequest(winner, 3));
            var invalidMove = game.Play(new PlayRequest(Player.Crosses, 4));

            // ASSERT
            Assert.True(invalidMove.ResultType == PlayResultType.GameIsOver);
        }

        [Fact]
        public void Play_PlayResultTypeIsInvalidPositionIfTheSquareIsAlreadyMarked()
        {
            // ARRANGE
            var currentPlayer = Player.Noughts;
            var game = new Game(currentPlayer);
            
            game.Board = new Dictionary<byte, Player>
            {
                { 1, Player.Crosses },
            };

            // ACT
            var playResult = game.Play(new PlayRequest(currentPlayer, 1));

            // ASSERT
            Assert.True(playResult.ResultType == PlayResultType.InvalidPosition);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        public void Play_PlayResultTypeIsInvalidPositionIfTheSquareDoesNotExist(byte position)
        {
            // ARRANGE
            var currentPlayer = Player.Noughts;
            var game = new Game(currentPlayer);

            // ACT
            var playResult = game.Play(new PlayRequest(currentPlayer, position));

            // ASSERT
            Assert.True(playResult.ResultType == PlayResultType.InvalidPosition);
        }

        [Fact]
        public void Play_PlayResultTypeIsWrongPlayerIfItIsNotThePlayersTurn()
        {
            // ARRANGE
            var currentPlayer = Player.Noughts;
            var game = new Game(currentPlayer);

            // ACT
            var playResult = game.Play(new PlayRequest(Player.Crosses, 1));

            // ASSERT
            Assert.True(playResult.ResultType == PlayResultType.WrongPlayer);
        }
    }
}
