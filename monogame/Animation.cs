using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame
{
    public class Animation
    {
        private Texture2D _spriteSheet;
        private int _frameCount;
        private int _frameWidth;
        private int _frameHeight;
        private float _frameTime;

        private int _currentFrame;
        private float _timer;

        public bool IsLooping { get; set; }
        public bool IsPlaying { get; private set; }

        public Animation(Texture2D spriteSheet, int frameCount, int frameWidth, int frameHeight, float frameTime, bool isLooping = true)
        {
            _spriteSheet = spriteSheet;
            _frameCount = frameCount;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _frameTime = frameTime;
            IsLooping = isLooping;

            _currentFrame = 0;
            _timer = 0f;
            IsPlaying = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsPlaying) return;

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer >= _frameTime)
            {
                _timer = 0f;
                _currentFrame++;

                if (_currentFrame >= _frameCount)
                {
                    if (IsLooping)
                    {
                        _currentFrame = 0;
                    }
                    else
                    {
                        _currentFrame = _frameCount - 1;
                        IsPlaying = false;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            // Bereken de source rectangle (welk frame op de spritesheet)
            Rectangle sourceRectangle = new Rectangle(
                _currentFrame * _frameWidth,
                0,
                _frameWidth,
                _frameHeight
            );

            spriteBatch.Draw(_spriteSheet, position, sourceRectangle, color);
        }

        public void Play()
        {
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
        }

        public void Reset()
        {
            _currentFrame = 0;
            _timer = 0f;
        }
    }
}
