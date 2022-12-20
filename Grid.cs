using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Sprity;

namespace Sty{
    public class Grid : ISprite {

        private Texture2D texture;
        private int cols = 100;
        private int rows = 100;
        private double centerX = 0;
        private double centerY = 0;
        private double gridSize = 10;
        private int width = 1680;
        private int height = 1050;
        private double scale = 1;
        private MouseState mouseState;
        private  MouseState previousState;
        private Vector2 mousePosition;
        private SpriteFont font;
        public Grid(SpriteFont font, GraphicsDevice graphicsDevice, int width, int height, GraphicsDeviceManager _graphics, int gridSize){
            texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.White });
            this.scale = (SpriteUtility.Instance.ScaleResolution);
            this.width = (int)(width / scale);
            this.height = (int)(height / scale);
            this.gridSize = gridSize / scale;
            this.rows = (this.height / gridSize) / 2;
            this.cols = (this.width  / gridSize) / 2;
            this.font = font;
            this.centerX = (this.width / 2);
            this.centerY = (this.height/ 2);	
        }   
        public void Update(double position, GameTime gameTime){
            previousState = mouseState;
            mouseState = Mouse.GetState(); //Needed to find the most current mouse states.
            mousePosition.X = mouseState.X; //Change x pos to mouseX
            mousePosition.Y = mouseState.Y;  
        }

        public void Draw(){
            SpriteUtility.Instance.SpriteBatch.Begin();
            for (float x = -cols; x < cols; x++)
            {
                var alpha = 0.1f;
                if(x == 0){
                    alpha = 0.6f;
                }
                Rectangle rectangle = new Rectangle((int)((centerX + 5) + x * gridSize), 0, 1, (height));
                SpriteUtility.Instance.SpriteBatch.Draw(texture, rectangle, Color.White * alpha);
            }
            for (float y = -rows; y < rows; y++)
            {
                var alpha = 0.1f;
                if(y == 0){
                    alpha = .6f;
                }
                Rectangle rectangle = new Rectangle(0, (int)(centerY + y * gridSize), (width), 1);
                SpriteUtility.Instance.SpriteBatch.Draw(texture, rectangle, null, Color.White * alpha);
            }
            SpriteUtility.Instance.SpriteBatch.DrawString(font, 
            "(X: " + Math.Round((mousePosition.X * scale) - 107, 2) + ",Y: " + Math.Round(mousePosition.Y * scale, 2) + ")", 
            new Vector2(0,0), 
            Color.White,
            0f, 
            new Vector2(0,0), 
            new Vector2(0.5f),
            SpriteEffects.None, 
            0);
            SpriteUtility.Instance.SpriteBatch.End();
        }
    }
}