using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Sprity;

namespace Sty {
    public class Project{
        private string _projectFolderPath;
        private string[] assets;
        private string _audioSourcePath;
        public string AudioSourcePath {get { return _audioSourcePath; }}
        private Dictionary<string, Texture2D> _textures;
        public Dictionary<string, Texture2D> Textures {get { return _textures; }}
        
        public Project(string path){
            this._projectFolderPath = path;
            LoadAssets();
            FindAssets();
        }
        public void LoadAssets(){
            assets = Directory.GetFiles(_projectFolderPath, "*.*", SearchOption.AllDirectories);  
        }

        public void FindAssets(){
            _textures = new Dictionary<string, Texture2D>();
            foreach(var file in assets){
                var path = Path.GetRelativePath(_projectFolderPath, file);
                var ext = Path.GetExtension(path);
                var key = path.ToLower();
                if(ext == ".mp3"){
                    _audioSourcePath = file;
                }
                if (ext == ".jpg" || ext == ".jpeg" || ext == ".png"){
                    var texture = Texture2D.FromFile(SpriteUtility.Instance.GraphicsDevice, file);
                    if(texture is not null){
                        Console.WriteLine(key);
                        _textures[key] = texture;
                    }
                }
            }
        }
    }
}