using System;
using System.Collections.Generic;
using System.Drawing;
using Sprity;

public class Particles {
    public List<Sprite> Generate(){
        var sprites = new List<Sprite>();


        for(var a = 0; a < 500; a++){
            var start = a * 600;
            var duration = 3500;
            for(var i = 0; i < 360 ; i+=(360 / 36)){
                var r = 854 - 320; //radius
                var radians = Math.PI * i / 180.0;
                var x = 320 + (r * Math.Cos(radians));
                var y = 240 + (r * Math.Sin(radians));
                
                var particle = new Sprite("window_04.png"){  Angle = radians, IsAdditiveBlend = true};
                particle.Fade(start, start + duration, 0, 1);
                particle.MoveX(start, start + duration, 320, x);
                particle.MoveY(start, start + duration, 240, y);
                particle.ScaleVec(start, start + duration, .1, .1, 1, .1);
                particle.Tint(255, 25, 224);
                //-sprites.Add(particle);
                start += 100;
            }
        }

        for(var a = 0; a < 500; a++){
            var start = a * 600;
            var duration = a * 10;
            
            for(var i = 360; i > 0 ; i-=(360 / 36)){
                var r = 854 - 220; //radius
                var radians = Math.PI * i / 180.0;
                var x = 320 + (r * Math.Cos(radians));
                var y = 240 + (r * Math.Sin(radians));
                
                var particle = new Sprite("spark_05.png"){ Angle = radians, IsAdditiveBlend = true};
                particle.Fade(start, start + duration, 0, 1);
                particle.MoveX(start, start + duration, 320, x);
                particle.MoveY(start, start + duration, 240, y);
                particle.ScaleVec(start, start + duration, .2, .2, 2, .2);
                particle.Tint(44, 25, 255);
                //sprites.Add(particle);
                start += 100;
            }
        }

        

        return sprites;
    }
}