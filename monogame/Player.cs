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
    public class Player : IUpdateable, IDrawable
    {
        private Animation _idleAnimation;
        private Animation _walkAnimation;
        private Animation _jumpAnimation;
        private Animation _currentAnimation;

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; private set; }
        public float Speed { get; set; }

        public float JumpStrength { get; set; }
        public float Gravity { get; set; }
        public bool IsOnGround { get; set; }
        private float _groundLevel;

        public bool IsMovingHorizontally { get; private set; }

        public int HitboxWidth { get; set; }
        public int HitboxHeight { get; set; }
        public int HitboxOffsetX { get; set; }
        public int HitboxOffsetY { get; set; }

        private KeyboardState _currentKeyboard;
        private KeyboardState _previousKeyboard;

        public Player(Vector2 startPosition, float groundLevel, Animation idleAnimation, Animation walkAnimation, Animation jumpAnimation)
        {
            Position = startPosition;
            Velocity = Vector2.Zero;
            Speed = 200f;
            JumpStrength = -500f;
            Gravity = 1500f;

            _groundLevel = groundLevel;
            IsOnGround = true;

            _idleAnimation = idleAnimation;
            _walkAnimation = walkAnimation;
            _jumpAnimation = jumpAnimation;
            _currentAnimation = _idleAnimation;

            HitboxWidth = 50;
            HitboxHeight = 120;
            HitboxOffsetX = 24;
            HitboxOffsetY = 8;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _previousKeyboard = _currentKeyboard;
            _currentKeyboard = Keyboard.GetState();

            Vector2 movement = Vector2.Zero;
            bool isMoving = false;

            // Horizontale beweging
            if (_currentKeyboard.IsKeyDown(Keys.A) || _currentKeyboard.IsKeyDown(Keys.Left))
            {
                movement.X -= 1;
                isMoving = true;
            }
            if (_currentKeyboard.IsKeyDown(Keys.D) || _currentKeyboard.IsKeyDown(Keys.Right))
            {
                movement.X += 1;
                isMoving = true;
            }

            IsMovingHorizontally = isMoving;

            // Jump input
            if ((_currentKeyboard.IsKeyDown(Keys.Space) || _currentKeyboard.IsKeyDown(Keys.W) || _currentKeyboard.IsKeyDown(Keys.Up))
                && _previousKeyboard.IsKeyUp(Keys.Space) && _previousKeyboard.IsKeyUp(Keys.W) && _previousKeyboard.IsKeyUp(Keys.Up)
                && IsOnGround)
            {
                Velocity = new Vector2(Velocity.X, JumpStrength);
                IsOnGround = false;
            }

            // Pas gravity toe
            if (!IsOnGround)
            {
                Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity * deltaTime);
            }

            // Normaliseer horizontale movement
            if (movement.X != 0)
            {
                movement.X = movement.X > 0 ? 1 : -1;
            }

            // Update horizontale positie
            Position += new Vector2(movement.X * Speed * deltaTime, 0);

            // Update verticale positie
            Position += new Vector2(0, Velocity.Y * deltaTime);

            // Grond detectie - GEFIXED ⬇️
            if (Position.Y >= _groundLevel)
            {
                Position = new Vector2(Position.X, _groundLevel);
                Velocity = new Vector2(Velocity.X, 0);
                IsOnGround = true;
            }
            else
            {
                // We zijn in de lucht (platforms zetten IsOnGround = true via CheckCollision)
                IsOnGround = false;
            }

            // Update animatie
            _currentAnimation.Update(gameTime);
        }

        public void UpdateAnimation(bool isMoving)
        {
            if (!IsOnGround)
            {
                if (_currentAnimation != _jumpAnimation)
                {
                    _currentAnimation = _jumpAnimation;
                    _currentAnimation.Reset();
                }
            }
            else if (isMoving)
            {
                if (_currentAnimation != _walkAnimation)
                {
                    _currentAnimation = _walkAnimation;
                    _currentAnimation.Reset();
                }
            }
            else
            {
                if (_currentAnimation != _idleAnimation)
                {
                    _currentAnimation = _idleAnimation;
                    _currentAnimation.Reset();
                }
            }
        }

        public void CheckCollision(IGameObject gameObject)
        {
            if (!gameObject.IsSolid) return;

            Rectangle playerBounds = GetBounds();
            Rectangle platformBounds = gameObject.Bounds;

            // CHECK 1: Zijn we AAN HET RAKEN van het platform? (intersects)
            if (playerBounds.Intersects(platformBounds))
            {
                // Bereken overlaps
                int overlapLeft = playerBounds.Right - platformBounds.Left;
                int overlapRight = platformBounds.Right - playerBounds.Left;
                int overlapTop = playerBounds.Bottom - platformBounds.Top;
                int overlapBottom = platformBounds.Bottom - playerBounds.Top;

                // Vind kleinste overlap
                int minOverlap = overlapTop;
                string direction = "top";

                if (overlapBottom < minOverlap)
                {
                    minOverlap = overlapBottom;
                    direction = "bottom";
                }
                if (overlapLeft < minOverlap)
                {
                    minOverlap = overlapLeft;
                    direction = "left";
                }
                if (overlapRight < minOverlap)
                {
                    minOverlap = overlapRight;
                    direction = "right";
                }

                // Reageer op collision
                if (direction == "top" && Velocity.Y >= 0)
                {
                    // Landing van boven
                    float newY = platformBounds.Top - HitboxHeight - HitboxOffsetY;
                    Position = new Vector2(Position.X, newY);
                    Velocity = new Vector2(Velocity.X, 0);
                    IsOnGround = true;
                }
                else if (direction == "bottom" && Velocity.Y < 0)
                {
                    // Hoofd tegen onderkant
                    float newY = platformBounds.Bottom - HitboxOffsetY;
                    Position = new Vector2(Position.X, newY);
                    Velocity = new Vector2(Velocity.X, 0);
                }
                else if (direction == "left")
                {
                    // Van links
                    Position = new Vector2(platformBounds.Left - HitboxWidth - HitboxOffsetX, Position.Y);
                }
                else if (direction == "right")
                {
                    // Van rechts
                    Position = new Vector2(platformBounds.Right - HitboxOffsetX, Position.Y);
                }
            }

            // CHECK 2: Staan we OP het platform? (ook als we niet intersecten) ⬇️ NIEUW!
            // Dit zorgt ervoor dat IsOnGround TRUE blijft als we erop staan
            Rectangle slightlyLowerBounds = new Rectangle(
                playerBounds.X,
                playerBounds.Y,
                playerBounds.Width,
                playerBounds.Height + 5  // Check 5 pixels onder ons
            );

            if (slightlyLowerBounds.Intersects(platformBounds) &&
                Math.Abs(playerBounds.Bottom - platformBounds.Top) <= 5)  // Maximaal 5 pixels van platform top
            {
                IsOnGround = true;
            }
        }

        public Rectangle GetBounds()
        {
            return new Rectangle(
                (int)Position.X + HitboxOffsetX,
                (int)Position.Y + HitboxOffsetY,
                HitboxWidth,
                HitboxHeight
            );
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _currentAnimation.Draw(spriteBatch, Position, Color.White);
        }
    }
}
