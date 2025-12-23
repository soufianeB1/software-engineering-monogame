using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame
{
    public class Enemy : IGameObject, IDrawable, IUpdateable
    {
        private Texture2D _texture;
        public Rectangle Bounds { get; private set; }
        public bool IsSolid { get; private set; }

        private Vector2 _position;
        private float _speed;
        private int _direction;  // 1 = rechts, -1 = links

        public Enemy(Texture2D texture, Vector2 startPosition, float speed = 50f)
        {
            _texture = texture;
            _position = startPosition;
            _speed = speed;
            _direction = 1;
            IsSolid = true;

            // Bijvoorbeeld 64x64 enemy
            Bounds = new Rectangle((int)_position.X, (int)_position.Y, 64, 64);
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Simpele AI: beweeg heen en weer
            _position.X += _direction * _speed * deltaTime;

            // Keer om bij schermrand (bijvoorbeeld)
            if (_position.X > 800) _direction = -1;
            if (_position.X < 200) _direction = 1;

            // Update bounds
            Bounds = new Rectangle((int)_position.X, (int)_position.Y, 64, 64);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(_texture, Bounds, Color.White);
        }

    }
}
