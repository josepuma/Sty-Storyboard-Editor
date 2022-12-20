using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprity;
namespace Sty {
    public class GuiControl {
        private Slider _slider;
        public GuiControl(){
            
        }

        public void Update(double musicPosition, double length){
            
        }


        public void Draw(){
            SpriteUtility.Instance.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            
            SpriteUtility.Instance.SpriteBatch.End();
        }
    }
}