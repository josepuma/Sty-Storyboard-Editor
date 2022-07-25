using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sprity;

namespace HelloWorld
{
   public class Demo
   {
       public List<Sprite> Generate()
       {
            var list = new List<Sprite>();
            Random rnd = new Random();
            var bg = new Sprite("rsz_bg.jpg"){ Size = 0.425 };
            bg.Fade(0,10000000,1,1);

            list.Add(bg);
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
                list.Add(sp);
                start+= 100;
            }
            return list;
       }

    public static double randomDouble(Random rand, double start, double end)
    {    
        return (rand.NextDouble() * Math.Abs(end-start)) + start;
    }
   }
} 