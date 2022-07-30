using System;
using System.Collections.Generic;
using System.Linq;
using Sprity;

public class Background {
    public List<Sprite> Generate(){
            var sprites = new List<Sprite>();
            var bg = new Sprite("sb/bg.png"){ Size = (854 / 1600.0) };
            bg.Fade(0,10000000,1,0); 
            //sprites.Add(bg);   

            var bg2 = new Sprite("BG.jpg"){ Size = 0.425 };
            bg2.Fade(0,10000000,1,0); 
            //sprites.Add(bg2);      

            var bg3 = new Sprite("light1.png"){ Size = 0.425, IsAdditiveBlend = true };
            bg3.Fade(0,10000000,1,0); 
            bg3.Rotate(0, 1000000, 1, 60); 
            //sprites.Add(bg3); 
            return sprites;
    }
}