using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame
{
    public class Coin : IGameObject, IDrawable
    {
        private Texture2D _texture;
        public Rectangle Bounds { get; private set; }
        public bool IsSolid { get; private set; }
        public bool IsCollected { get; private set; }

        public Coin(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            Bounds = new Rectangle((int)position.X, (int)position.Y, 32, 32);
            IsSolid = false;  // Je kan er doorheen lopen!
            IsCollected = false;
        }

        public void Collect()
        {
            IsCollected = true;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!IsCollected)
            {
                spriteBatch.Draw(_texture, Bounds, Color.Gold);
            }
        }
    }
}
