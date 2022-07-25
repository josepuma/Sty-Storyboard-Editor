﻿using System;
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
using Sprity;namespace Sty
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

       public List<Sprite> CompileCode()
        {
            ScriptOptions scriptOptions = ScriptOptions.Default;
            //Add reference to mscorlib
            var mscorlib = typeof(System.Object).Assembly;
            var systemCore = typeof(System.Linq.Enumerable).Assembly;
            var sprity = typeof(Sprity.Sprite).Assembly;
            scriptOptions = scriptOptions.AddReferences(mscorlib, systemCore, sprity);
            //Add namespaces
            scriptOptions = scriptOptions.AddImports("System");
            scriptOptions = scriptOptions.AddImports("System.Linq");
            scriptOptions = scriptOptions.AddImports("System.Collections.Generic");
            scriptOptions = scriptOptions.AddImports("Sprity");



            var script = File.ReadAllText("scripts/DemoStoryboard.cs");

            //var state = await CSharpScript.RunAsync(script, scriptOptions);
            var list = CSharpScript.RunAsync(script, scriptOptions).Result
                       .ContinueWithAsync<List<Sprite>>("new Storyboard().Generate()").Result.ReturnValue;
            return list;
        }

        protected override void LoadContent()
        {
          
        
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            var song = "/Applications/osu!w.app/Contents/Resources/drive_c/osu!/Songs/33810 Hashimoto Miyuki - NEVERLAND/NEVERLAND.mp3";
            _mainBackgroundSong = new Sound(song);
            _texturesContent = TextureContent.LoadListContent<Texture2D>(GraphicsDevice, "/Users/josepuma/Documents/personal/sb");
            SpriteUtility.Instance.SetSpriteBatch(_spriteBatch);
            SpriteUtility.Instance.SetContentTextures(_texturesContent);
            SpriteUtility.Instance.SetGraphicsContext(_graphics);
            SpriteUtility.Instance.SetGraphicsDevice(GraphicsDevice);
            
            sbObjects = new List<Sprite>();
            sbObjects.AddRange(CompileCode());

            font = Content.Load<SpriteFont>("assets/Fonts/Arial");
            _grid = new Grid(font, GraphicsDevice, 854, 480, _graphics , 10);
            
            
           

            _mainBackgroundSong.Play();

        }

        protected override void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(_mainBackgroundSong.IsPlaying){
                var _soundPosition = _mainBackgroundSong.GetPosition();
                if(Keyboard.GetState().IsKeyDown(Keys.Right))
                    _mainBackgroundSong.ChangePosition(_soundPosition + 1000);

                if(Keyboard.GetState().IsKeyDown(Keys.Left))
                    _mainBackgroundSong.ChangePosition(_soundPosition - 1000);


                foreach (Sprite spriteObject in sbObjects)
                {
                    spriteObject.Update(_soundPosition, gameTime);
                }
            }
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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
