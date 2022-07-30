using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Threading.Tasks;
using Sprity;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Sty {
    public class ScriptLoader{

        private string[] scriptPaths;
        private Dictionary<string, string> _scripts;

        private string _path;
        
        private List<Sprite> _sprites;
        public ScriptLoader(string path){
            _scripts = new Dictionary<string, string>();
            _path = path;
            
            _sprites = new List<Sprite>();
            LoadScripts();
        }

        public void LoadScripts(){
            _scripts.Clear();
            var paths = Directory.EnumerateFiles(_path, "*.cs").ToList();
            Parallel.ForEach(paths, current => 
            {
                var code = File.ReadAllText(current);
                var key = Path.GetFileNameWithoutExtension(current);
                if(!String.IsNullOrEmpty(code) && code.Contains("Generate")){
                    if(_scripts.ContainsKey(key)){
                        _scripts[key] = code;
                    }else{
                        _scripts.Add(key, code);
                    }
                }
                
            });
        }

        public List<Sprite> CompileCode(){
            ScriptOptions scriptOptions = ScriptOptions.Default;
            //Add reference to mscorlib
            var mscorlib = typeof(System.Object).Assembly;
            var systemCore = typeof(System.Linq.Enumerable).Assembly;
            var sprity = typeof(Sprity.Sprite).Assembly;
            scriptOptions = scriptOptions.AddReferences(mscorlib, systemCore, sprity);
            //Add namespaces
            scriptOptions = scriptOptions.AddImports("System");
            scriptOptions = scriptOptions.AddImports("System.Linq");
            scriptOptions = scriptOptions.AddImports("System.Collections.Generic");
            scriptOptions = scriptOptions.AddImports("Sprity");

            _sprites.Clear();

            foreach(var script in _scripts){
                if(script.Value is not null){
                    var list = CSharpScript.RunAsync(script.Value, scriptOptions).Result
                       .ContinueWithAsync<List<Sprite>>("new " + script.Key + "().Generate()").Result.ReturnValue;
                       if(list.Count > 0){
                            _sprites.AddRange(list);
                       }
                }
                
            }
            return _sprites;
        }

        public async Task<List<Sprite>> GetSpritesAsync(){
            return await Task.FromResult(CompileCode());
        }

    }
}