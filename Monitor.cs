using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using Sprity;

namespace Sty {
    public class Monitor : IDisposable
  {
     private List<string> filePaths;
     private System.Threading.ReaderWriterLockSlim rwlock;
     private Timer processTimer;
     private string watchedPath;
     private FileSystemWatcher watcher;
   
     private List<Sprite> sprites;

     private ScriptLoader _scriptLoader;

    DateTime lastRead = DateTime.MinValue;

    public Monitor(string watchedPath, List<Sprite> sbObjects, ScriptLoader scriptLoader)
    {
        filePaths = new List<string>();
        this.sprites = sbObjects;
        rwlock = new System.Threading.ReaderWriterLockSlim();
        filePaths = GetFiles(watchedPath);
        _scriptLoader = scriptLoader;
        this.watchedPath = watchedPath;
        InitFileSystemWatcher();
    }

     private void InitFileSystemWatcher()
     {
        watcher = new FileSystemWatcher();
        watcher.Filter = "*.cs*";
        watcher.Created += Watcher_FileCreated;
        watcher.Error += Watcher_Error;
        watcher.Path = watchedPath;
        watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;
        watcher.Changed += Watcher_FileChanged;
        watcher.NotifyFilter = NotifyFilters.LastWrite;
     }

    private void Watcher_Error(object sender, ErrorEventArgs e)
    {
        // Watcher crashed. Re-init.
        InitFileSystemWatcher();
    }
    public event EventHandler OnFileChanged;
   protected virtual void OnFileChange(EventArgs e){
      OnFileChanged.Invoke(this, e);
   }
    private void Watcher_FileChanged(object sender, FileSystemEventArgs e){
      DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath);
      if (lastWriteTime != lastRead)
      {
         OnFileChange(e);
         lastRead = lastWriteTime;
      }
        //CompileCode();
    }
    private List<string> GetFiles(string directory){
        var myFiles = Directory.EnumerateFiles(directory, "*.cs*").ToList();
        return myFiles;
    }
    private void CompileCode(){
        
    }

     private void Watcher_FileCreated(object sender, FileSystemEventArgs e)
     {
        try
        {
           rwlock.EnterWriteLock();
           filePaths.Add(e.FullPath);

           if (processTimer == null)
           {
              // First file, start timer.
              processTimer = new Timer(2000);
              processTimer.Elapsed += ProcessQueue;
              processTimer.Start();
           }
           else
           {
              // Subsequent file, reset timer.
              processTimer.Stop();
              processTimer.Start();
           }

        }
        finally
        {
           rwlock.ExitWriteLock();
        }
     }

     private void ProcessQueue(object sender, ElapsedEventArgs args)
     {
        try
        {
           Console.WriteLine("Processing queue, " + filePaths.Count + " files created:");
           rwlock.EnterReadLock();
           foreach (string filePath in filePaths)
           {
              Console.WriteLine("process queue:" + filePath);
           }
           filePaths.Clear();
        }
        finally
        {
           if (processTimer != null)
           {
              processTimer.Stop();
              processTimer.Dispose();
              processTimer = null;
           }
           rwlock.ExitReadLock();
        }
     }

     protected virtual void Dispose(bool disposing)
     {
        if (disposing)
        {
           if (rwlock != null)
           {
              rwlock.Dispose();
              rwlock = null;
           }
           if (watcher != null)
           {
              watcher.EnableRaisingEvents = false;
              watcher.Dispose();
              watcher = null;
           }
        }
     }

     public void Dispose()
     {
        Dispose(true);
        GC.SuppressFinalize(this);
     }

  }     
}