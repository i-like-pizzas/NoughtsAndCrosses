using Domain.Models;

namespace ApplicationServices.Services
{
    public class GameService : IGameService
    {
        public IGame NewGame()
        {
            var random = new Random();
            var player = random.Next(0, 2) == 0 ? Player.Crosses : Player.Noughts;

            return new Game(player);
        }
    }
}
