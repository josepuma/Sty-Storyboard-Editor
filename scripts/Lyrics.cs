using System;
using Storyboard;
using System.Collections.Generic;
namespace Script
{
    public class Lyrics
    {
        public void Generate(List<Sprite> sprites)
        {
            var sprite = new Sprite("blue.png");
            sprites.Add(sprite);       
        }
    }
}