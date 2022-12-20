using System;
using System.Collections.Generic;
using System.Linq;
using Sprity;
using TinyTween;

public class Dream {
    public List<Sprite> Generate(){
            var sprites = new List<Sprite>();
            Random rnd = new Random();
            sprites.Add(Background());
            sprites.AddRange(Sparkles()); 

            var r = 360; //radius
            var startx = 320;
            var starty = 240;
            
            for(var i = 0; i < 360 ; i+=(360 / 36)){
                var radians = Math.PI * i / 180.0;
                var x = startx + (r * Math.Cos(radians));
                var y = starty + (r * Math.Sin(radians));
                var duration = (int)randomDouble(rnd, 100, 840);

                var spark = new Sprite("sb/star.png"){ IsAdditiveBlend = true, Size = .3 };
                spark.Fade(0, 3000, 1,1);
                //spark.MoveX(ScaleFuncs.SineIn, 0, 4000, startx, x);
                spark.MoveX(0, 3000, startx, x);
                spark.MoveY(0, 3000, starty, y);
                //spark.MoveY(ScaleFuncs.SineIn, 3000, 4000, y, 500);
                sprites.Add(spark);
            }

            return sprites;
    }

    public Sprite Background(){
        var bg = new Sprite("bg.jpg"){ Size = (854 / 1500.0) };
        bg.Fade(0,10000000,.1,0); 
        return bg;
    }

    public List<Sprite> Sparkles(){
        Random rnd = new Random();
        var sparkles = new List<Sprite>();
        var r = 450; //radius
        var startx = 320;
        var starty = 240;
            for(var start = 27079; start < 48712; start+=168){
                for(var i = 0; i < 360 ; i+=(360 / 48)){
                    var duration = (int)randomDouble(rnd, 100, 840);

                    var radians = Math.PI * i / 180.0;
                    var x = startx + (r * Math.Cos(radians));
                    var y = starty + (r * Math.Sin(radians));
                    if(y > 320){
                        var x2 = x + (r * Math.Cos(radians));
                        var y2 = randomDouble(rnd, 320, 500);
                        var s  = randomDouble(rnd, 0, .1);
                        var spark = new Sprite("sb/spark.png"){ IsAdditiveBlend = true, Size = s, Angle = radians };
                        spark.Fade(start, start + duration * 2, 1, 1);
                        spark.MoveX(ScaleFuncs.SineOut,start, start + duration * 2, startx, x);
                        spark.MoveY(ScaleFuncs.SineIn,start, start + duration * 2, starty, y);
                        spark.Scale(start, start + duration * 2, s, 0);
                        spark.Tint(230, 172, 80);
                        sparkles.Add(spark);
                    }
                    
                }
            }

            for(var start = 27079; start < 48712; start+=168 * 2){
                for(var i = 0; i < 360 ; i+=(360 / 36)){
                    var duration = (int)randomDouble(rnd, 400, 1840);

                    var radians = Math.PI * i / 180.0;
                    var x = startx + (r * Math.Cos(radians));
                    var y = starty + (r * Math.Sin(radians));

                    if(y > 320){
                        var x2 = x + (r * Math.Cos(radians));
                        var y2 = randomDouble(rnd, 320, 500);
                        var s  = randomDouble(rnd, .01, .25);
                        var spark = new Sprite("sb/star.png"){ IsAdditiveBlend = true, Size = s, Angle = radians };
                        spark.Fade(start, start + duration * 2, 1, 1);
                        spark.MoveX(ScaleFuncs.SineOut, start, start + duration * 2, startx, x);
                        spark.MoveY(ScaleFuncs.SineIn,start, start + duration * 2, starty, y);
                        spark.Scale(start, start + duration * 2, s, 0);
                        spark.Rotate(start, start + duration * 2, radians, radians * 2);
                        spark.Tint(230, 172, 80);
                        sparkles.Add(spark);
                    }   

                    
                }
            }

            var angle = Math.PI * 45 / 180.0;
            var angle2 = Math.PI * 135 / 180.0;

            var flare = new Sprite("sb/starflare.png"){ IsAdditiveBlend = true, Size = 2, Angle = angle };
            flare.Fade(0,100000,1,1);
            flare.Move(0,100000,startx, starty, startx, starty);
            flare.Rotate(0,100000, angle, angle2);
            flare.Tint(80, 177, 230);
            sparkles.Add(flare);

            
            var flare2 = new Sprite("sb/starflare.png"){ IsAdditiveBlend = true, Size = 2, Angle = angle2 };
            flare2.Fade(0,100000,1,1);
            flare2.Move(0,100000,startx, starty, startx, starty);
            flare2.Rotate(0,100000, angle2, angle);
            flare2.Tint(80, 177, 230);
            sparkles.Add(flare2);
        return sparkles;
    }

    public double randomDouble(Random rand, double start, double end)
    {    
        return (rand.NextDouble() * Math.Abs(end-start)) + start;
    }
}