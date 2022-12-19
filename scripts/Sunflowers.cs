using System;
using System.Collections.Generic;
using System.Linq;
using Sprity;

public class Sunflowers {
    public List<Sprite> Generate(){
            Random rnd = new Random();
            var sprites = new List<Sprite>();

            var bg2 = new Sprite("bg.jpg"){ Size = (854 / 1920.0) };
            bg2.Fade(0,10000000,0,1); 
            //-sprites.Add(bg2);   
            
            var start = 0;
            var duration = 5600;
            var introDuration = 650;
            var step = 10;
            var quantity = 2000;
            var stepPosition = 20;
            for(var i = 0; i<quantity; i++){
                var x = randomDouble(rnd, -127, 760);
                var y = randomDouble(rnd, 320, 480);
                var r = randomDouble(rnd, 0, 6);
                var r2 = randomDouble(rnd, 0, 6);
                var s  = randomDouble(rnd, 0, .3);
                var x2 = randomDouble(rnd, x - stepPosition, x + stepPosition);
                var y2 = randomDouble(rnd, y - stepPosition, y + stepPosition);
                var sunflower = new Sprite("sunflower.png"){ Size = .2 };
                sunflower.Fade(start, start + duration,1,1);
                sunflower.Fade((start + duration) - 400, start + duration,1,0);
                sunflower.MoveX(start, start + duration, x, x2);
                sunflower.MoveY(start, start + duration, y, y2);
                sunflower.Rotate(start, start + introDuration, r, r2);
                sunflower.Scale(start, start + introDuration, 0, s);
                //sprites.Add(sunflower);
                start+=step;
            }

            
            return sprites;
    }

    public static double randomDouble(Random rand, double start, double end)
    {    
        return (rand.NextDouble() * Math.Abs(end-start)) + start;
    }
}