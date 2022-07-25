using System;
using System.IO;
using ManagedBass;

namespace Sty{
    public class Sound {
        bool _isPlaying;
        bool _isStreamLoaded;
        int _stream;
        public bool IsPlaying {get { return _isPlaying; }}
        public Sound(string path){
            Bass.Init();
            if(File.Exists(path)){
                _stream = Bass.CreateStream(path);
                Console.WriteLine("file:" +  path + _stream);
                if(_stream != 0){
                    _isStreamLoaded = true;

                }
            }
        }

        public void Play(){
            if(_isStreamLoaded){
                Bass.ChannelPlay(_stream);
                _isPlaying = true;
            }
        }

        public double GetPosition(){
            if(_isStreamLoaded && _isPlaying){
                var bytesPosition = Bass.ChannelGetPosition(_stream);
                var time = Bass.ChannelBytes2Seconds(_stream, bytesPosition);
                return time * 1000;
            }
            return 0;
        }

        public void ChangePosition(double position){
            var newPosition = Bass.ChannelSeconds2Bytes(_stream, position / 1000);
            Bass.ChannelSetPosition(_stream, newPosition);
        }
    }
}