using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sty {
   public static class TextureContent
    {
        public static Dictionary<string, Texture2D> LoadListContent<T>(GraphicsDevice graphicsDevice, string contentFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
            Dictionary<String, Texture2D> result = new Dictionary<String, Texture2D>();

            FileInfo[] files = dir.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string key = file.Name;
                string path = Path.Combine(contentFolder, key);
                FileStream filestream = new FileStream(path, FileMode.Open);
                Texture2D myTexture = Texture2D.FromStream(graphicsDevice, filestream);
                result[key] = myTexture;
            }
            return result;
        }
    }  
}