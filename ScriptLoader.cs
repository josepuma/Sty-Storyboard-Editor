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
        private Monitor _monitor;
        private string _path;
        
        private List<Sprite> _sprites;
        public ScriptLoader(string path, List<Sprite> sprites){
            _scripts = new Dictionary<string, string>();
            _path = path;
            _monitor = new Monitor("scripts", this);
            _sprites = sprites;
            LoadScripts();
        }

        public void LoadScripts(){
            var paths = Directory.EnumerateFiles(_path, "*.cs").ToList();
            Parallel.ForEach(paths, current => 
            {
                var code = File.ReadAllText(current);
                var key = Path.GetFileNameWithoutExtension(current);
                if(_scripts.ContainsKey(key)){
                    _scripts[key] = code;
                }else{
                    _scripts.Add(key, code);
                }
            });
        }

        public void CompileCode(){
            Console.WriteLine("compiling code"); 
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

            foreach(var script in _scripts){
                var list = CSharpScript.RunAsync(script.Value, scriptOptions).Result
                       .ContinueWithAsync<List<Sprite>>("new " + script.Key + "().Generate()").Result.ReturnValue;
                _sprites.AddRange(list);
            }
        }

    }
}