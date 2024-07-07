using ApplicationServices.Services;
using Microsoft.Extensions.DependencyInjection;

namespace NoughtsAndCrosses
{
    internal class Program
    {
        static ServiceProvider? serviceProvider;

        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IGameService, GameService>();

            serviceProvider = serviceCollection.BuildServiceProvider();

            RunGame();
        }

        static void RunGame()
        {
            var gameService = serviceProvider.GetService<IGameService>();
        }
    }
}
