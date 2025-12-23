using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame
{
    public interface IDrawable
    {
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
