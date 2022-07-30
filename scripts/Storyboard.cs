using System;
using System.Collections.Generic;
using System.Linq;
using Sprity;

public class Storyboard {
    public List<Sprite> Generate(){
            var sprites = new List<Sprite>();
            


            //Background
            Random rnd = new Random();

            //Particles
            var start = 0;
            var beat = 13405;
            foreach (var i in Enumerable.Range(0, 100000))
            {
                var x = rnd.Next(-127, 854);
                var y = rnd.Next(0, 480);
                var r = rnd.Next(0, 60);
                var s = randomDouble(rnd, 0.01, .02);
                var f = randomDouble(rnd, 0.1, 1);
                var sp = new Sprite("light2.png"){ IsAdditiveBlend = true };
                sp.Scale(start, start + beat,s,s);
                sp.Rotate(start, start + beat,r,0);
                sp.Move(start, start + beat,x, 580, x,0);
                sp.Fade(start, start + beat,f,0);
                //sprites.Add(sp);
                start+= 100;
            }

            

            var gradient = new Sprite("gradient.png"){ IsAdditiveBlend = true, Opacity = 0.6 };
            gradient.ScaleVec(0, 100000, 854, 1, 854, 1);
            gradient.MoveY(0, 10000, 360, 360);
            //sprites.Add(gradient);

            return sprites;
        }


    public static double randomDouble(Random rand, double start, double end)
    {    
        return (rand.NextDouble() * Math.Abs(end-start)) + start;
    }
}