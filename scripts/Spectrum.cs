using System;
using System.Collections.Generic;
using System.Linq;
using Sprity;

public class Spectrum {
    public List<Sprite> Generate(){
            var sprites = new List<Sprite>();
            // Spectrum
            var song = "/Users/josepuma/Downloads/The Only One I Need - Maxi Malone.mp3";
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

                    sprites.Add(bar);
                    pos++;
                }
            return sprites;
    }
}