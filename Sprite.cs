using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sty {
    public class Sprite : ISprite
    {
        private Texture2D texture;
        private Vector2 position = new Vector2(427, 240);
        private Color color = Color.White;
        private Vector2 size = new Vector2(0.625f);
        private Vector2 origin;
        
        public Sprite(string path)
        {
            texture = SpriteUtility.Instance.Textures[path];
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }
        
        public void Update(GameTime gameTime)
        {
            size.X += 0.0001f;
            size.Y += 0.0001f;    
        }
        
        public void Draw()
        {
            SpriteUtility.Instance.SpriteBatch.Begin();
            
            SpriteUtility.Instance.SpriteBatch.Draw(texture, position / (float)SpriteUtility.Instance.ScaleResolution, null, color, 0f, origin, size / (float)SpriteUtility.Instance.ScaleResolution, SpriteEffects.None, 0f);
            
            SpriteUtility.Instance.SpriteBatch.End();
        }
    }
}