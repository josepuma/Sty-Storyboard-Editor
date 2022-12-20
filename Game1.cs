using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ManagedBass;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleFpsCounter;
using Sprity;

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
        private Monitor _monitor;
        private ScriptLoader _scriptLoader;
        private bool _areSpritesReloading;
        private Project _project;
        private Slider _slider; 
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "/Users/josepuma/Documents/personal/mono/Sty";
            IsMouseVisible = true;
                       
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            _graphics.PreferredBackBufferWidth = 1366;
            _graphics.PreferredBackBufferHeight = 768;
            //_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //_graphics.IsFullScreen = true;
            _graphics.SynchronizeWithVerticalRetrace = false;
            this.IsFixedTimeStep = false;
            _graphics.ApplyChanges();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteUtility.Instance.SetSpriteBatch(_spriteBatch);
            SpriteUtility.Instance.SetGraphicsContext(_graphics);
            SpriteUtility.Instance.SetGraphicsDevice(GraphicsDevice);
            
            

            _project = new Project("/Applications/osu!w.app/Contents/Resources/drive_c/osu!/Songs/151720 ginkiha - EOS");
            SpriteUtility.Instance.SetContentTextures(_project.Textures);
            

            sbObjects = new List<Sprite>();
            
           
            
            _mainBackgroundSong = new Sound(_project.AudioSourcePath);
            Console.WriteLine(_project.AudioSourcePath);

            _scriptLoader = new ScriptLoader("scripts");
            _monitor = new Monitor("scripts", sbObjects, _scriptLoader);
            _monitor.OnFileChanged += Monitor_ReloadSprites;
            sbObjects = _scriptLoader.GetSpritesAsync().Result;
            

            font = Content.Load<SpriteFont>("assets/Fonts/Arial");
            _grid = new Grid(font, GraphicsDevice, 854, 480, _graphics , 10);
            _slider = new Slider(font);
            _slider.Changed += UpdateSongPosition;
            
           

            _mainBackgroundSong.Play();

        }

        private void UpdateSongPosition(double pos){
            _mainBackgroundSong.ChangePosition(pos);
        }


        private void Monitor_ReloadSprites(object sender, EventArgs e){
            ReloadSprites();
        }

        private void ReloadSprites(){
            _areSpritesReloading = true;
            sbObjects.Clear();
            _scriptLoader.LoadScripts();
            var sprites = _scriptLoader.GetSpritesAsync().Result;
            sbObjects = sprites.ToList();  
            sbObjects = sbObjects.ToList();
            _areSpritesReloading = false;
        }

        protected override void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(_mainBackgroundSong.IsPlaying){
                var _soundPosition = _mainBackgroundSong.GetPosition();
                var _length = _mainBackgroundSong.GetLength();
                if(Keyboard.GetState().IsKeyDown(Keys.Right))
                    _mainBackgroundSong.ChangePosition(_soundPosition + 1000);

                if(Keyboard.GetState().IsKeyDown(Keys.Left))
                    _mainBackgroundSong.ChangePosition(_soundPosition - 1000);

                if(sbObjects.Count > 0){
                    for(var i = 0; i < sbObjects.Count; i++){
                        var spriteObject = sbObjects[i];
                        spriteObject.Update(_soundPosition, gameTime);
                    }
                }
                _slider.Update(_soundPosition, _length);
                
            }
            if(Keyboard.GetState().IsKeyDown(Keys.Space))
                _mainBackgroundSong.Pause();
            
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _grid.Update(0, gameTime);
            fps.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if(sbObjects.Count > 0){
                 for(var i = 0; i < sbObjects.Count; i++){
                    var spriteObject = sbObjects[i];
                    spriteObject.Draw();
                }
            }
            
            
            if(Keyboard.GetState().IsKeyDown(Keys.G)){
                _grid.Draw();
                fps.DrawFps(_spriteBatch, font, new Vector2(10f, 10f), Color.MonoGameOrange);
            }

            _slider.Draw();

            base.Draw(gameTime);
        }
    }
}
