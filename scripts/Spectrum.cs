using System;
using System.Collections.Generic;
using Sprity;
using System.Linq;

namespace HelloWorld
{
    public class Animation
   {
        public List<Sprite> Generate()
        {
            var list = new List<Sprite>();

            var song = "/Applications/osu!w.app/Contents/Resources/drive_c/osu!/Songs/151720 ginkiha - EOS/eos.mp3";
            var streamfft = new FftSound(song);
            var st = 0;
            var ed = streamfft.Duration * 1000;
            var barCount = 2;
            var offset = 50;
            var scale = 10;

            var keys = new Spectrum<BarFFt>(barCount);
            var barfft = new BarFFt();
            barfft.AddFft(new Fft
            {
                StartTime = 1000,
                StartValue = 1
            });
            
            keys.Set(barfft, 0);

            var info = keys.GetAt(0);
            //Console.WriteLine(info.GetNumbers());

            List<BarFFt> values = new List<BarFFt>();
            for(var i = 0; i < barCount; i++){
                values.Add(
                    new BarFFt()
                );
            }
            for(var time = st; time < ed ; time += offset){
                var fft = streamfft.GetFft(time);
                for(var i = 0; i < barCount; i++){
                    /*values[i].Add(
                        new Fft(){
                            StartTime = (int)time,
                            StartValue = .1 + (float)Math.Log10(1 + fft[i] * scale) * 10
                        }
                    );*/                    
                }
            }
            
            var pos = 1;
            //Console.WriteLine(bars[0].GetNumbers());
            //var n = ff.GetNumbers();
            foreach(var fft in values){
                var bar = new Sprite("bar.png"){ IsAdditiveBlend = true };
                bar.Move(0,1000000, 10 * pos, 400, 10*pos, 400);
                bar.Fade(0,1000000,1,1);
                //fft.GetNumbers();
                //fft.GetNumbers();
                //fft.GetTimes();
                //var times = fft.GetTimes();
                //Console.WriteLine(t);
                /*for(var i = 0; i < times.Count; i++){
                    if(i + 1 < times.Count){
                        var startTime = times[i].StartTime;
                        var startValue = times[i].StartValue;
                        var endTime = times[i + 1].StartTime;
                        var endValue = times[i + 1].StartValue;
                        bar.ScaleVec(startTime, endTime, .8, startValue, .8, endValue);
                    }
                }*/

                list.Add(bar);
                pos++;
            }
            return list;
        }

        public static double randomDouble(Random rand, double start, double end)
        {    
            return (rand.NextDouble() * Math.Abs(end-start)) + start;
        }
   }
}