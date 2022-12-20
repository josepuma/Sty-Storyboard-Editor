using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprity;
namespace Sty {
    public class Slider {
        private Texture2D texture;
        private Vector2 origin;
        private Vector2 position = new Vector2(320 + 107,450);
        private Color color = Color.White;
        private float opacity = 1f;
        private float rotation = 0;
        private Vector2 size = new Vector2(.2f);
        private Vector2 sizeBg = new Vector2(20f, .05f);
        private Rectangle bbox;
        private bool dragging;
        private double songPosition = 0;
        private MouseState preMouse;
        public event SendNewMusicPositionHandler Changed;
        private SpriteFont font;

        public Slider(SpriteFont font){
            this.font = font;
            if (SpriteUtility.Instance.Textures.ContainsKey("sb/d.png"))
            {
                texture = SpriteUtility.Instance.Textures["sb/d.png"];
                origin = new Vector2(texture.Width / 2, texture.Height / 2);
            }
        }

        public delegate void SendNewMusicPositionHandler(double newPosition);

        public void Update(double musicPosition, double length){
            
            if(musicPosition > 0){
                var progress = (musicPosition * 100 / length);
                var value = MathHelper.Lerp(0, 854.0f, (float)(progress / 100));
                songPosition = musicPosition;
                rotation += .01f;
                var posX = (int)((position.X - texture.Width / 2) / SpriteUtility.Instance.ScaleResolution);
                var posY = (int)((position.Y - texture.Height / 2) / SpriteUtility.Instance.ScaleResolution);

                bbox = new Rectangle(posX, posY, texture.Width, texture.Height);

                MouseState mouseState = Mouse.GetState();

                if(bbox.Contains(new Point((int)mouseState.X, (int)mouseState.Y))){
                    if(mouseState.LeftButton == ButtonState.Pressed){
                        var x = mouseState.X;
                        position.X = x * (float)SpriteUtility.Instance.ScaleResolution;
                        float musicProgress = (float)(position.X * 100 / 854) / 100.0f;
                        songPosition = (double)MathHelper.Lerp(0, (float)length, musicProgress);
                        dragging = true;
                        Changed(songPosition);
                    }else{
                        dragging = false;
                    }
                }

                if(!dragging)
                    position.X = value;
            }
            

            
        }


        public void Draw(){
            SpriteUtility.Instance.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            SpriteUtility.Instance.SpriteBatch.Draw(texture,
            new Vector2(427,450) / (float)SpriteUtility.Instance.ScaleResolution,
            null,
            color * opacity,
            0,
            origin,
            sizeBg / (float)SpriteUtility.Instance.ScaleResolution,
            SpriteEffects.None,
            0f);
            SpriteUtility.Instance.SpriteBatch.Draw(texture,
            position / (float)SpriteUtility.Instance.ScaleResolution,
            null,
            color * opacity,
            rotation,
            origin,
            size / (float)SpriteUtility.Instance.ScaleResolution,
            SpriteEffects.None,
            0f);
            TimeSpan ts = TimeSpan.FromMilliseconds(songPosition);
            SpriteUtility.Instance.SpriteBatch.DrawString(font, ts.ToString(@"mm\:ss"), new Vector2(427, 427) / (float)SpriteUtility.Instance.ScaleResolution, Color.White, 0f, new Vector2(0, 0), new Vector2(.8f),SpriteEffects.None, 0);
            SpriteUtility.Instance.SpriteBatch.End();
        }
    }
}