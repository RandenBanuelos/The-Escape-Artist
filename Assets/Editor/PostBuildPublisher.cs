// Define build platforms this project supports.
#if UNITY_EDITOR_WIN
#define WINDOWS_BUILD_SUPPORT
#elif UNITY_EDITOR_OSX
#define OSX_BUILD_SUPPORT
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MichaelWolfGames;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PostBuildPublisher
{
    private static string PROJECT_NAME = "TheEscapeArtist_Prototype";

    private static string DEV_DEBUG_BUILD_SYMBOL = "DEV_DEBUG_BUILD";
    
    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) 
    {
        Debug.Log(("Built Project Path: " + pathToBuiltProject).RichText(Color.yellow));

        if (target == BuildTarget.WebGL)
        {
            Debug.Log("Built WebGL!".RichText(Color.cyan));
        }
        else if (target == BuildTarget.StandaloneWindows)
        {
            Debug.Log("Built StandaloneWindows!".RichText(Color.cyan));
        }
        else if (target == BuildTarget.StandaloneOSX)
        {
            Debug.Log("Built StandaloneOSX!".RichText(Color.cyan));
        }
    }

#region Windows
#if WINDOWS_BUILD_SUPPORT
    
    private static string windowsBuildFolderName => PROJECT_NAME + "_Dev";
    
    [MenuItem("Build/Windows (Dev)/Build and Publish")]
    public static void BuildAndPublishWindows()
    {
        // Increment Version Number
        IncrementVersionNumber();
        
        // // Update version text file used when publishing with butler.
        // string buildFolderPath = Application.dataPath.Replace("/Assets", "/Builds/Windows/");
        // File.WriteAllText(buildFolderPath + "buildnumber.txt", string.Format("{0}", Application.version));
        
        BuildWindows(() =>
        {
            PublishWindows();
        });
    }
    
    [MenuItem("Build/Windows (Dev)/Build")]
    public static void BuildWindows()
    {
        BuildWindows(null);
    }
    
    private static void BuildWindows(Action onBuildSuccessful = null)
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName =  "Builds/Windows/" + windowsBuildFolderName +"/" + PROJECT_NAME + ".exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows;
        buildPlayerOptions.options = BuildOptions.None;
        
        // Get & Set Scripting Define Symbols
        string[] originalSymbols;
        PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, out originalSymbols);
        List<string> buildSymbols = new List<string>(originalSymbols);
        buildSymbols.Add(DEV_DEBUG_BUILD_SYMBOL);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, buildSymbols.ToArray());
        
        // DEFAULT SCENES FROM BUILD SETTINGS
        List<string> scenes = new List<string>();
        scenes.AddRange(EditorBuildSettings.scenes.Where(scene => scene.enabled)
            .Select(scene => scene.path).ToArray());
        
        // Load dynamic level scenes.
        //scenes.AddRange(GetLevelScenesForBuild());
        
        // Apply to buildPlayerOptions.
        buildPlayerOptions.scenes = scenes.ToArray();
        
        // BUILD THE PLAYER
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;
        
        // Reset Scripting Define Symbols
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, originalSymbols);
        
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            
            onBuildSuccessful?.Invoke();
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
    
    [MenuItem("Build/Windows (Dev)/Publish")]
    public static void PublishWindows()
    {
        Debug.Log("Publishing Last Windows Build.");
        
        // Update version text file used when publishing with Butler.
        string buildFolderPath = Application.dataPath.Replace("/Assets", "/Builds/Windows/");
        File.WriteAllText(buildFolderPath + "buildnumber.txt", string.Format("{0}", Application.version));
        
        // Run the shell script.
        string arguments = string.Format("\"{0}\" \"{1}\"", (buildFolderPath + windowsBuildFolderName), (buildFolderPath + "buildnumber.txt"));
        RunShellScript("Builds/Windows/Butler_Publish.sh", arguments);
    }

    [MenuItem("Build/Windows (Dev)/Publish (Manual Bash)")]
    public static void PublishWindows_ManualBash()
    {
        RunShellScript("Builds/Windows/Butler_Publish_Manual.sh");
    }
    
#endif // WINDOWS_BUILD_SUPPORT
#endregion

#region Mac
#if OSX_BUILD_SUPPORT
    
    private static string macBuildFolderName => PROJECT_NAME + "_Dev";
    
    [MenuItem("Build/Mac (Dev)/Build and Publish")]
    public static void BuildAndPublishMac()
    {
        // Increment Version Number
        IncrementVersionNumber();
        
        // // Update version text file used when publishing with butler.
        // string buildFolderPath = Application.dataPath.Replace("/Assets", "/Builds/Mac/");
        // File.WriteAllText(buildFolderPath + "buildnumber.txt", string.Format("{0}", Application.version));
        
        BuildMac(() =>
        {
            PublishMac();
        });
    }
    
    [MenuItem("Build/Mac (Dev)/Build")]
    public static void BuildMac()
    {
        BuildMac(null);
    }
    
    private static void BuildMac(Action onBuildSuccessful = null)
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName =  "Builds/Mac/" + macBuildFolderName +"/" + PROJECT_NAME + ".exe";
        buildPlayerOptions.target = BuildTarget.StandaloneOSX;
        buildPlayerOptions.options = BuildOptions.None;
        
        // Get & Set Scripting Define Symbols
        string[] originalSymbols;
        PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, out originalSymbols);
        List<string> buildSymbols = new List<string>(originalSymbols);
        buildSymbols.Add(DEV_DEBUG_BUILD_SYMBOL);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, buildSymbols.ToArray());
        
        // DEFAULT SCENES FROM BUILD SETTINGS
        List<string> scenes = new List<string>();
        scenes.AddRange(EditorBuildSettings.scenes.Where(scene => scene.enabled)
            .Select(scene => scene.path).ToArray());
        
        // Load dynamic level scenes.
        //scenes.AddRange(GetLevelScenesForBuild());
        
        // Apply to buildPlayerOptions.
        buildPlayerOptions.scenes = scenes.ToArray();
        
        // BUILD THE PLAYER
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;
        
        // Reset Scripting Define Symbols
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, originalSymbols);
        
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            
            onBuildSuccessful?.Invoke();
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
    
    [MenuItem("Build/Mac (Dev)/Publish")]
    public static void PublishMac()
    {
        Debug.Log("Publishing Last Mac Build.");
        
        // Update version text file used when publishing with Butler.
        string buildFolderPath = Application.dataPath.Replace("/Assets", "/Builds/Mac/");
        File.WriteAllText(buildFolderPath + "buildnumber.txt", string.Format("{0}", Application.version));
        
        // Run the shell script.
        string arguments = string.Format("\"--{0}\" \"--{1}\"", (buildFolderPath + macBuildFolderName), (buildFolderPath + "buildnumber.txt"));
        RunCommandScript("Builds/Mac/Butler_Publish.command", arguments);
    }

    [MenuItem("Build/Mac (Dev)/Publish (Manual Bash)")]
    public static void PublishMac_ManualBash()
    {
        Debug.Log("Publishing Last Mac Build (MANUAL BASH)");
        RunCommandScript("Builds/Mac/Butler_Publish_Manual.command");
    }
    
#endif // OSX_BUILD_SUPPORT
#endregion // Mac
    
#region WebGL
#if WEBGL_BUILD_SUPPORT
    private static string webGLBuildFolderName => PROJECT_NAME + "_Dev";

    [MenuItem("Build/WebGL (Dev)/Build and Publish")]
    public static void BuildAndPublishWebGL()
    {
        // Update version text file used when publishing with butler.
        string buildFolderPath = Application.dataPath.Replace("/Assets", "/Builds/WebGL/");
        File.WriteAllText(buildFolderPath + "buildnumber.txt", string.Format("{0}", Application.version));
        
        BuildWebGL(() =>
        {
            PublishWebGL();
        });
    }

    [MenuItem("Build/WebGL (Dev)/Build")]
    public static void BuildWebGL()
    {
        BuildWebGL(null);
    }
    
    private static void BuildWebGL(Action onBuildSuccessful = null)
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        //buildPlayerOptions.locationPathName = "Builds/WebGL/" + PROJECT_NAME + "_Dev";
        buildPlayerOptions.locationPathName =  "Builds/WebGL/" + webGLBuildFolderName;
        buildPlayerOptions.target = BuildTarget.WebGL;
        buildPlayerOptions.options = BuildOptions.None;
        
        // DEFAULT SCENES FROM BUILD SETTINGS
        List<string> scenes = new List<string>();
        scenes.AddRange(EditorBuildSettings.scenes.Where(scene => scene.enabled)
            .Select(scene => scene.path).ToArray());
        
        // Load dynamic level scenes.
        //scenes.AddRange(GetLevelScenesForBuild());
        
        // Apply to buildPlayerOptions.
        buildPlayerOptions.scenes = scenes.ToArray();
        
        
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            
            onBuildSuccessful?.Invoke();
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
    
        


    // private static List<string> GetLevelScenesForBuild()
    // {
    //     List<string> scenes = new List<string>();
    //     LevelCollection levelCollection = Resources.Load<LevelCollection>("LevelCollection");
    //     foreach (LevelData level in levelCollection.levels)
    //     {
    //         if (level.scene != null && level.scene.scenePath != "")
    //         {
    //             scenes.Add("Assets/" + level.scene.scenePath + ".unity");
    //         }
    //     }
    //     return scenes;
    // }
    
    [MenuItem("Build/WebGL (Dev)/Publish")]
    public static void PublishWebGL()
    {
        Debug.Log("Publishing Last Build.");
        
        string buildFolderPath = Application.dataPath.Replace("/Assets", "/Builds/WebGL/");
        string arguments = string.Format("\"{0}\" \"{1}\"", (buildFolderPath + webGLBuildFolderName), (buildFolderPath + "buildnumber.txt"));
        RunShellScript("Builds/WebGL/Butler_Publish.sh", arguments);
    }

    [MenuItem("Build/WebGL (Dev)/Publish (Manual Bash)")]
    public static void PublishWebGL_ManualBash()
    {
        RunShellScript("Builds/WebGL/Butler_Publish_Manual.sh");
    }
    
#endif //WEBGL_BUILD_SUPPORT
#endregion

#region Shell Scripts
    
    public static void RunShellScript(string relativePath, string arguments = null)
    {
        string projectPath = Application.dataPath.Replace("/Assets", "/");
        
        Process proc = new Process();
        ProcessStartInfo procStartInfo = new ProcessStartInfo
        {
            FileName = projectPath + relativePath,
            UseShellExecute = true,
            RedirectStandardOutput = false,
            CreateNoWindow = true,
        };
        if (!string.IsNullOrEmpty(arguments))
        {
            procStartInfo.Arguments = arguments;
        }
        proc.StartInfo = procStartInfo;
        proc.Start();
    }
    
    public static void RunCommandScript(string relativePath, string arguments = null)
    {
        string projectPath = Application.dataPath.Replace("/Assets", "/");
        //System.Diagnostics.Process.Start(@"/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal");

        Process proc = new Process();
        ProcessStartInfo procStartInfo = new ProcessStartInfo
        {
            FileName = "/bin/sh",
            Arguments = "\"" + projectPath + relativePath + "\" " + ((!string.IsNullOrEmpty(arguments))? arguments : ""),
            //FileName = projectPath + relativePath,
            //Arguments = ((!string.IsNullOrEmpty(arguments))? arguments : ""),
            
            //FileName = "open",
            //FileName = "open \"" + projectPath + relativePath + "\"",
            //FileName = "open \"" + projectPath + relativePath + "\"",
            
            //Arguments = "Terminal",
            //Arguments = ((!string.IsNullOrEmpty(arguments))? arguments : ""),
            //Arguments = "sh \"" + projectPath + relativePath + "\" " + ((!string.IsNullOrEmpty(arguments))? arguments : ""),
            //Arguments = "\"" + projectPath + relativePath + "\" --args " + ((!string.IsNullOrEmpty(arguments))? arguments : ""),
            //Arguments = "\"" + projectPath + relativePath + "\" --args " + ((!string.IsNullOrEmpty(arguments))? arguments : ""),
            //Arguments = "\"" + projectPath + relativePath + "\" --args " + ((!string.IsNullOrEmpty(arguments))? arguments : ""),
            
            UseShellExecute = false,
            //RedirectStandardInput = true,
            //RedirectStandardOutput = false,
            RedirectStandardOutput = true,
            //CreateNoWindow = false,
        };
        

        // arguments = "-a Terminal.app " + ((!string.IsNullOrEmpty(arguments))? arguments : "");
        //
        // if (!string.IsNullOrEmpty(arguments))
        // {
        //     procStartInfo.Arguments = arguments;
        // }
        proc.StartInfo = procStartInfo;
        
        Debug.Log(proc.StartInfo.Arguments);
        
        proc.ErrorDataReceived += (sender, args) =>
        {
            Debug.Log("ErrorDataReceived: " + args);
        };
        proc.Exited += (sender, args) =>
        {
            Debug.Log("Exited: " + args);
        };
        proc.OutputDataReceived += (sender, args) =>
        {
            Debug.Log("OutputDataReceived: " + args);
        };
        proc.Disposed += (sender, args) =>
        {
            Debug.Log("Disposed: " + args);
        };
        proc.Start();
        
        //proc.StandardInput.WriteLine("\"" + projectPath + relativePath + "\" " + ((!string.IsNullOrEmpty(arguments))? arguments : ""));
    }

    private static void IncrementVersionNumber()
    {
        // Note: PlayerSettings.bundleVersion == Application.version
        string[] versionNums = PlayerSettings.bundleVersion.Split('.');
        if(Int32.TryParse(versionNums[versionNums.Length-1], out int patch))
        {
            versionNums[versionNums.Length - 1] = (++patch).ToString();
        }
        string version = "";
        for (int i = 0; i < versionNums.Length; i++)
        {
            version += versionNums[i].ToString();
            if (i < versionNums.Length - 1)
            {
                version += ".";
            }
        }
        PlayerSettings.bundleVersion = version;
    }
    
#endregion //Shell Scripts
}
