using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace monogame
{
    public class MainMenuState : IGameState
    {

        private Texture2D _backgroundTexture;
        private ContentManager _content;
        private GraphicsDevice _graphicsDevice;
        private StateManager _stateManager;

        private List<Button> _buttons;

        public MainMenuState(ContentManager content, GraphicsDevice graphicsDevice, StateManager stateManager)
        {
            _content = content;
            _graphicsDevice = graphicsDevice;
            _stateManager = stateManager;
            _buttons = new List<Button>();
        }

        public void LoadContent()
        {
            // Laad achtergrond
            _backgroundTexture = _content.Load<Texture2D>("menu-background-FINAL");

            // Laad levels button texture (vervang met jouw naam!)
            Texture2D levelsButtonTexture = _content.Load<Texture2D>("menu-button-levels");

            // Maak Levels button
            Button levelsButton = new Button(
                levelsButtonTexture,
                new Rectangle(
                    _graphicsDevice.Viewport.Width / 2 - 100,
                    _graphicsDevice.Viewport.Height / 2 - 50,
                    200,
                    100
                )
            );

            levelsButton.OnClick += () =>
            {
                // Ga naar level select screen! 🎯
                LevelSelectState levelSelect = new LevelSelectState(_content, _graphicsDevice, _stateManager);
                _stateManager.ChangeState(levelSelect);
            };

            _buttons.Add(levelsButton);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var button in _buttons)
            {
                button.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(
                _backgroundTexture,
                new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height),
                Color.White
            );

            foreach (var button in _buttons)
            {
                button.Draw(spriteBatch);
            }

            spriteBatch.End();

        }
    }
    }

