using UnityEditor;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace GUPU {
    public static class PackageLock {
        public static Dictionary<string, Dependency> File {
            get {
                string json = System.IO.File.ReadAllText(GetPackageLockPath());
                return JsonConvert.DeserializeObject<Object>(json).dependencies;
            }
            set {
                Object lockObject = new Object();
                lockObject.dependencies = value;
                string json = JsonConvert.SerializeObject(lockObject, Formatting.Indented);
                System.IO.File.WriteAllText(GetPackageLockPath(), json);
                AssetDatabase.Refresh();
            }
        }
        
        public struct Object {
            public Dictionary<string, Dependency> dependencies;
        }
    
        public struct Dependency {
            public string version;
            public int depth;
            public string source;
            public Object dependencies;
            public string hash;
        } 
        
        private static string GetPackageLockPath() => (
            System.IO.Path.GetFullPath(System.IO.Path.Combine(
                Application.dataPath, "../Packages/packages-lock.json"
            ))
        );
    }
}