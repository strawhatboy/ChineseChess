using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Strawhat.Games
{
    public class GameBase : IGame
    {
        public GameStatus LastGameStatus { set; get; }

        public GameStatus CurrentGameStatus { set; get; }

        public void ChangeGameStatus(GameStatus newGameStatus)
        {
            this.OnGameStatusChanged(this.CurrentGameStatus, newGameStatus);
        }

        public virtual void OnGameStatusChanged(GameStatus lastGameStatus, GameStatus newGameStatus)
        {
            if (this.GameStatusChanged != null)
                this.GameStatusChanged(lastGameStatus, newGameStatus);
            this.CurrentGameStatus = newGameStatus;
            this.LastGameStatus = this.CurrentGameStatus;
        }

        public Action<GameStatus, GameStatus> GameStatusChanged;
    }
}
