using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sty {
    public sealed class SpriteUtility
    {
        private static SpriteUtility instance = null;
        private static readonly object _lock = new object();
        
        public ContentManager ContentManager { get; private set; }

        public Dictionary<string, Texture2D> Textures { get; private set;}
        
        public SpriteBatch SpriteBatch { get; private set; }

        public double ScaleResolution {get; private set;}
        
        public static SpriteUtility Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new SpriteUtility();
                    }
                    
                    return instance;
                }
            }
        }
        
        public void SetContentTextures(Dictionary<string, Texture2D> textures)
        {
            this.Textures = textures;
        }
        
        public void SetSpriteBatch(SpriteBatch spriteBatch)
        {
            this.SpriteBatch = spriteBatch;
        }

        public void SetGraphicsContext(GraphicsDeviceManager graphicsDevice){
            this.ScaleResolution = 480.0f / graphicsDevice.PreferredBackBufferHeight;
        }

        public SpriteUtility(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            this.ContentManager = contentManager;
            this.SpriteBatch = spriteBatch;
        }

        public SpriteUtility()
        {
            
        }
    }
}