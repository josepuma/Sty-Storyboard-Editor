using System;
using System.Collections.Generic;
using System.IO;
using ManagedBass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleFpsCounter;
using Sprity;
using Westwind.Scripting;

namespace Sty
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private List<Sprite> sbObjects;
        private Dictionary<string, Texture2D> _texturesContent;
        private Grid _grid;
        private Sound _mainBackgroundSong;
        private SpriteFont font;
        SimpleFps fps = new SimpleFps();
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "/Users/josepuma/Documents/personal/mono/Sty";
            IsMouseVisible = true;
                       
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            _graphics.SynchronizeWithVerticalRetrace = false;
            this.IsFixedTimeStep = false;
            base.Initialize();
        }

       

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _mainBackgroundSong = new Sound("/Applications/osu!w.app/Contents/Resources/drive_c/osu!/Songs/35701 Lia - Toki wo Kizamu Uta/Lia-Toki wo Kizamu Uta.mp3");
            _texturesContent = TextureContent.LoadListContent<Texture2D>(GraphicsDevice, "/Users/josepuma/Documents/personal/sb");
            SpriteUtility.Instance.SetSpriteBatch(_spriteBatch);
            SpriteUtility.Instance.SetContentTextures(_texturesContent);
            SpriteUtility.Instance.SetGraphicsContext(_graphics);
            
            sbObjects = new List<Sprite>();
            /*sbObjects.Add(
                new Sprite("rsz_bg.jpg")
            );*/
 

            var script = new CSharpScriptExecution(){ SaveGeneratedCode = true };
            script.AddDefaultReferencesAndNamespaces();
            script.AddAssembly(typeof(Sprity.Sprite));
            
            script.AddNamespace("Storyboard");
            
            
            var code = File.ReadAllText("scripts/Background.cs");
            var sb = script.CompileClass(code);
            var sprites = sb.Generate();
            sbObjects.AddRange(sprites);
            //var properties = sprite.GetType().GetProperties();

            Console.WriteLine("Error: " + script.ErrorMessage);
            Console.WriteLine("Error: " + script.ErrorType);
            Console.WriteLine("Error: " + script.Error);
            font = Content.Load<SpriteFont>("assets/Fonts/Arial");
            _grid = new Grid(font, GraphicsDevice, 854, 480, _graphics , 10);
            _mainBackgroundSong.Play();
        
        }

        protected override void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(_mainBackgroundSong.IsPlaying){
                var _soundPosition = _mainBackgroundSong.GetPosition();
            
                foreach (Sprite spriteObject in sbObjects)
                {
                    spriteObject.Update(_soundPosition, gameTime);
                }
            }
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            
            _grid.Update(0, gameTime);
            fps.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            foreach (Sprite spriteObject in sbObjects)
            {
                spriteObject.Draw();
            }
            
            if(Keyboard.GetState().IsKeyDown(Keys.G)){
                _grid.Draw();
                fps.DrawFps(_spriteBatch, font, new Vector2(10f, 10f), Color.MonoGameOrange);
            }
            
            base.Draw(gameTime);
        }
    }
}
