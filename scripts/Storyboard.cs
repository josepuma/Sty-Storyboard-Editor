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

            // Spectrum
            var song = "/Applications/osu!w.app/Contents/Resources/drive_c/osu!/Songs/574067 Tia - The Glory Days/audio.mp3";
            var streamfft = new FftSound(song);
            var st = 0;
            var ed = streamfft.Duration * 1000;
            var barCount = 55;
            var offset = 50;
            var scale = 10;
            
            var keys = new Spectrum<BarFFt>(barCount);
            List<BarFFt> values = new List<BarFFt>();
            for(var i = 0; i < barCount; i++){
                keys.Set(new BarFFt(), i);
            }

            for(var time = st; time < ed ; time += offset){
                    var fft = streamfft.GetFft(time);
                    for(var i = 0; i < barCount; i++){
                        keys.GetAt(i).AddFft(
                            new Fft(){
                                StartTime = (int)time,
                                StartValue = .1 + (float)Math.Log10(1 + fft[i] * scale) * 10
                            }
                        );                 
                    }
            }

            var pos = 1; 
                for(var j = 0; j< barCount; j++){
                    var fft = keys.GetAt(j);
                    var bar = new Sprite("bar.png"){ IsAdditiveBlend = true };
                    bar.Move(0,1000000, 10 * pos, 400, 10*pos, 400);
                    bar.Fade(0,1000000,1,1);
                    var times = fft.GetTimes();
                    for(var i = 0; i < times.Count; i++){
                        if(i + 1 < times.Count){
                            var startTime = times[i].StartTime;
                            var startValue = times[i].StartValue;
                            var endTime = times[i + 1].StartTime;
                            var endValue = times[i + 1].StartValue;
                            bar.ScaleVec(startTime, endTime, .8, startValue, .8, endValue);
                        }
                    }

                    //sprites.Add(bar);
                    pos++;
                }

            return sprites;
        }


    public static double randomDouble(Random rand, double start, double end)
    {    
        return (rand.NextDouble() * Math.Abs(end-start)) + start;
    }
}