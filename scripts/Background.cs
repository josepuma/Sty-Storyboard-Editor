using System;
using System.IO;
using Sprity;

namespace HelloWorld
{
   public class Demo
   {
       public Sprite Generate()
       {
            var sp = new Sprite("light2.png"){ };
            sp.Fade(0,10000,1,0);
            sp.Scale(0,10000,1,0);
            return sp;
       }
   }
}