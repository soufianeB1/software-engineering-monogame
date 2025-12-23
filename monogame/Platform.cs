using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame
{
    public class Platform : IGameObject , IDrawable
    {
        private Texture2D _texture;
        public Rectangle Bounds { get; private set; }
        public bool IsSolid { get; private set; }

        public Platform(Texture2D texture, Rectangle bounds, bool isSolid = true)
        {
            _texture = texture;
            Bounds = bounds;
            IsSolid = isSolid;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(_texture, Bounds, Color.White);
        }
    }
}
