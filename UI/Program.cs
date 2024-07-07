using ApplicationServices.Services;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;

namespace NoughtsAndCrosses
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IGameService, GameService>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            RunGame(serviceProvider);
        }

        static void MainMenu(IGameService gameService)
        {
            do
            {
                Console.Write(GetMenu());

                string? input = Console.ReadLine()?.ToUpper();

                if (input == "N")
                {
                    NewGame(gameService);
                    break;
                }
                    
            } while (true);
        }

        /// <summary>
        /// Returns a nought or cross.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        static string RenderMark(Player player)
        {
            if (player == Player.Crosses)
                return "X";
            else
                return "O";
        }

        /// <summary>
        /// Returns a board with a nought, cross, or dash based on what mark is in the specified position.
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        static void DrawBoard(IGame game)
        {
            var board = new Dictionary<byte, string>();

            for(byte i = 1; i < 10; i++)
                board.Add(i, game.Board.ContainsKey(i) ? RenderMark(game.Board[i]) : "-");

            var chunks = board.Chunk(3);

            foreach(var chunk in chunks)
                Console.WriteLine(string.Join(" | ", chunk.SelectMany(c => c.Value)));
        }

        static void NewGame(IGameService gameService)
        {
            Console.Clear();
            var game = gameService.NewGame();
            var gameIsOver = false;

            do
            {
                Console.WriteLine($"{game.CurrentPlayer}' turn. Enter a value between 1 and 9");

                byte? input = ParsePlayerInput(Console.ReadLine()?.ToUpper());

                if(input.HasValue)
                {
                    var result = game.Play(new PlayRequest(game.CurrentPlayer, input.Value));

                    if(result.ResultType == PlayResultType.Success)
                    {
                        Console.WriteLine(result.GameStatus);

                        if (result.GameStatus != GameStatus.InProgress)
                            gameIsOver = true;

                        DrawBoard(game);
                    }
                    else
                    {
                        Console.WriteLine(result.ResultType);
                    }
                }
            }
            while (!gameIsOver);

            MainMenu(gameService);
        }

        static byte? ParsePlayerInput(string? input)
        {
            if (!string.IsNullOrEmpty(input) && byte.TryParse(input, out byte result))
                return result;

            return null;
        }

        static void RunGame(IServiceProvider serviceProvider)
        {
            var gameService = serviceProvider.GetService<IGameService>();

            MainMenu(gameService ?? throw new NotImplementedException());
        }

        static string GetMenu()
        {
            var sb = new StringBuilder();

          
            sb.AppendLine("Instructions");
            sb.AppendLine("Enter a value of 1 to 9 to mark a square on the grid.");
            sb.AppendLine("1 | 2 | 3");
            sb.AppendLine("4 | 5 | 6");
            sb.AppendLine("7 | 8 | 9");

            sb.AppendLine();
            sb.AppendLine("Enter 'N' to start new game");

            return sb.ToString();
        }
    }
}
