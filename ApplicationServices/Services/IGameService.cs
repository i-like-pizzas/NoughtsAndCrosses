using ApplicationServices.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Services
{
    public interface IGameService
    {
        PlayResult Play(PlayRequest request);

        string GetInstructions();
    }
}
