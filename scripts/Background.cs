using System;
using System.Collections.Generic;
using System.Linq;
using Sprity;

public class Background {
    public List<Sprite> Generate(){
            var sprites = new List<Sprite>();
            var bg = new Sprite("bakaliz3.jpg"){ Size = 0.425 };
            bg.Fade(0,10000000,1,1); 
            bg.Scale(0, 100000, 1, 1);
            sprites.Add(bg);    
            return sprites;
    }
}