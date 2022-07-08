using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sty
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private List<ISprite> sbObjects;
        private Dictionary<string, Texture2D> _texturesContent;
        private Grid _grid;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "/Users/josepuma/Documents/mono/Sty";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _texturesContent = TextureContent.LoadListContent<Texture2D>(GraphicsDevice, "/Applications/osu!w.app/Contents/Resources/drive_c/osu!/Songs/151720 ginkiha - EOS/sb");
            SpriteUtility.Instance.SetSpriteBatch(_spriteBatch);
            SpriteUtility.Instance.SetContentTextures(_texturesContent);
            SpriteUtility.Instance.SetGraphicsContext(_graphics);
            
            sbObjects = new List<ISprite>();
           
            
            _grid = new Grid(Content.Load<SpriteFont>("assets/Fonts/Arial"), GraphicsDevice, 854, 480, _graphics , 10);
            
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (ISprite spriteObject in sbObjects)
            {
                spriteObject.Update(gameTime);
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            
            _grid.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            foreach (ISprite spriteObject in sbObjects)
            {
                spriteObject.Draw();
            }
            
            if(Keyboard.GetState().IsKeyDown(Keys.G)){
                _grid.Draw();
            }
            
            base.Draw(gameTime);
        }
    }
}
