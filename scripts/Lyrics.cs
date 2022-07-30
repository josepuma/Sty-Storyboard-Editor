using System.Collections.Generic;
using System.Drawing;
using Sprity;

public class Lyrics {
    public List<Sprite> Generate(){
        var sprites = new List<Sprite>();
        
        var lyric = new Sprite("sb/306a.png"){  Position = new Point(320, 240) };
        lyric.Fade(0, 10000, 1, 1);
        lyric.Scale(0, 10000, 20, 1);
        

        sprites.Add(lyric);

        return sprites;
    }
}