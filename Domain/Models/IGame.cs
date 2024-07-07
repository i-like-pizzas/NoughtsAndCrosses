using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public interface IGame
    {
        PlayResult Play(PlayRequest request);

        Player CurrentPlayer { get; }

        GameStatus GameStatus { get; }

        Dictionary<byte, Player> Board { get; }
        Player? Winner { get; }
    }
}
