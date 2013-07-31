using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Strawhat.Games
{
    public interface IGame
    {
        GameStatus LastGameStatus { set; get; }
        GameStatus CurrentGameStatus { set; get; }
        void ChangeGameStatus(GameStatus newGameStatus);
    }
}
