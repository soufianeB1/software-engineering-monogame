using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame
{
    public class GamePlayState : IGameState
    {
        private Texture2D _backgroundTexture;
        private ContentManager _content;
        private GraphicsDevice _graphicsDevice;
        private StateManager _stateManager;

        private Player _player;

        // Dependency Inversion: We gebruiken interfaces, niet concrete types!
        private List<IGameObject> _gameObjects;  // ✅ ALLE objecten!
        private List<IUpdateable> _updateables;
        private List<IDrawable> _drawables;

        public GamePlayState(ContentManager content, GraphicsDevice graphicsDevice, StateManager stateManager)
        {
            _content = content;
            _graphicsDevice = graphicsDevice;
            _stateManager = stateManager;

            _gameObjects = new List<IGameObject>();
            _updateables = new List<IUpdateable>();
            _drawables = new List<IDrawable>();
        }

        public void LoadContent()
        {
            _backgroundTexture = _content.Load<Texture2D>("level1-background");

            // Laad textures
            Texture2D platformTexture = _content.Load<Texture2D>("platform1");
            Texture2D playerIdleSheet = _content.Load<Texture2D>("Idle");
            Texture2D playerWalkSheet = _content.Load<Texture2D>("Walk");

            // Player animaties
            Animation idleAnimation = new Animation(playerIdleSheet, 6, 128, 128, 0.15f, true);
            Animation walkAnimation = new Animation(playerWalkSheet, 10, 128, 128, 0.1f, true);
            Animation jumpAnimation = new Animation(playerWalkSheet, 10, 128, 128, 0.1f, false);

            float groundLevel = _graphicsDevice.Viewport.Height - 128 - 50;
            Vector2 startPosition = new Vector2(100, groundLevel);

            _player = new Player(startPosition, groundLevel, idleAnimation, walkAnimation, jumpAnimation);
            _updateables.Add(_player);
            _drawables.Add(_player);

            // Maak platforms (IGameObject!)
            AddPlatform(platformTexture, new Rectangle(300, 600, 200, 50));
            AddPlatform(platformTexture, new Rectangle(500, 550, 200, 50));
            AddPlatform(platformTexture, new Rectangle(700, 600, 200, 50));


            // Later: Voeg enemies toe!
            // Texture2D enemyTexture = _content.Load<Texture2D>("enemy");
            // AddEnemy(enemyTexture, new Vector2(400, 350));

            // Later: Voeg coins toe!
            // Texture2D coinTexture = _content.Load<Texture2D>("coin");
            // AddCoin(coinTexture, new Vector2(350, 350));
        }

        // Helper methods (Single Responsibility!)
        private void AddPlatform(Texture2D texture, Rectangle bounds)
        {
            Platform platform = new Platform(texture, bounds);
            _gameObjects.Add(platform);  // ✅ IGameObject
            _drawables.Add(platform);    // ✅ IDrawable
        }

        private void AddEnemy(Texture2D texture, Vector2 position)
        {
            Enemy enemy = new Enemy(texture, position);
            _gameObjects.Add(enemy);   // ✅ IGameObject
            _updateables.Add(enemy);   // ✅ IUpdateable
            _drawables.Add(enemy);     // ✅ IDrawable
        }

        private void AddCoin(Texture2D texture, Vector2 position)
        {
            Coin coin = new Coin(texture, position);
            _gameObjects.Add(coin);    // ✅ IGameObject
            _drawables.Add(coin);      // ✅ IDrawable
        }

        public void Update(GameTime gameTime)
        {
            // Update alle updateable objecten
            
            
                // Update alle updateable objecten
                foreach (var updateable in _updateables)
                {
                    updateable.Update(gameTime);
                }

                // Check collision met ALLE game objecten
                foreach (var gameObject in _gameObjects)
                {
                    _player.CheckCollision(gameObject);
                }

                // Update animatie NA collision check!
                _player.UpdateAnimation(_player.IsMovingHorizontally);
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(
                _backgroundTexture,
                new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height),
                Color.White
            );

            // Teken alle drawable objecten
            foreach (var drawable in _drawables)
            {
                drawable.Draw(spriteBatch, new GameTime());
            }

            spriteBatch.End();
        }
    }
}
