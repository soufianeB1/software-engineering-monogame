using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame
{
    public class Button
    {
        private Texture2D _texture;
        private Rectangle _rectangle;

        private MouseState _currentMouse;
        private MouseState _previousMouse;

        public event System.Action OnClick;

        public Button(Texture2D texture, Rectangle rectangle)
        {
            _texture = texture;
            _rectangle = rectangle;
        }

        public void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            Rectangle mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            if (mouseRectangle.Intersects(_rectangle))
            {
                if (_currentMouse.LeftButton == ButtonState.Released &&
                    _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    OnClick?.Invoke();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, Color.White);
        }
    }
}

