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
    public class LevelSelectState : IGameState
    {
        private Texture2D _backgroundTexture;
        private ContentManager _content;
        private GraphicsDevice _graphicsDevice;
        private StateManager _stateManager;

        private List<Button> _buttons;

        public LevelSelectState(ContentManager content, GraphicsDevice graphicsDevice, StateManager stateManager)
        {
            _content = content;
            _graphicsDevice = graphicsDevice;
            _stateManager = stateManager;
            _buttons = new List<Button>();
        }

        public void LoadContent()
        {
            // Laad achtergrond (zelfde als main menu)
            _backgroundTexture = _content.Load<Texture2D>("menu-background-FINAL");

            // Laad button textures
            Texture2D level1Texture = _content.Load<Texture2D>("level1-button");
            Texture2D level2Texture = _content.Load<Texture2D>("level2-button");
            Texture2D level3Texture = _content.Load<Texture2D>("level3-button");

            int screenCenterX = _graphicsDevice.Viewport.Width / 2;
            int buttonWidth = 200;
            int buttonHeight = 100;
            int spacing = 120; // Ruimte tussen buttons

            // Level 1 button
            Button level1Button = new Button(
                level1Texture,
                new Rectangle(
                    screenCenterX - buttonWidth / 2,
                    200,  // Y positie
                    buttonWidth,
                    buttonHeight
                )
            );
            level1Button.OnClick += () =>
            {
                System.Diagnostics.Debug.WriteLine("Level 1 selected!");
                // Start level 1! 🎮
                GamePlayState gamePlay = new GamePlayState(_content, _graphicsDevice, _stateManager);
                _stateManager.ChangeState(gamePlay);
            };
            _buttons.Add(level1Button);

            // Level 2 button
            Button level2Button = new Button(
                level2Texture,
                new Rectangle(
                    screenCenterX - buttonWidth / 2,
                    200 + spacing,  // Onder level 1
                    buttonWidth,
                    buttonHeight
                )
            );
            level2Button.OnClick += () =>
            {
                System.Diagnostics.Debug.WriteLine("Level 2 selected!");
                // Later: start level 2
            };
            _buttons.Add(level2Button);

            // Level 3 button
            Button level3Button = new Button(
                level3Texture,
                new Rectangle(
                    screenCenterX - buttonWidth / 2,
                    200 + spacing * 2,  // Onder level 2
                    buttonWidth,
                    buttonHeight
                )
            );
            level3Button.OnClick += () =>
            {
                System.Diagnostics.Debug.WriteLine("Level 3 selected!");
                // Later: start level 3
            };
            _buttons.Add(level3Button);
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

            // Teken achtergrond
            spriteBatch.Draw(
                _backgroundTexture,
                new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height),
                Color.White
            );

            // Teken alle buttons
            foreach (var button in _buttons)
            {
                button.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
