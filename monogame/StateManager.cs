using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static monogame.IGameState;

namespace monogame
{
    public class StateManager
    {
        private IGameState _currentState;

        public void ChangeState(IGameState newState)
        {
            _currentState = newState;
            _currentState.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            _currentState?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentState?.Draw(spriteBatch);
        }
    }
}
