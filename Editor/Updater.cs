using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace GUPU {
    public static class Updater {
        public static string GetLastCommitHash(string repoUrl) {
            Process process = new Process();
            process.StartInfo.FileName = "git";
            process.StartInfo.Arguments = $"ls-remote {repoUrl}";
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
            string output = process.StandardOutput.ReadToEnd();
            string hash = output.Substring(0, 40);
            return hash;
        }
    }
}