using UnityEngine;
using System.IO;

public class IPathTools  {
    public static string GetPlatformFolderName(RuntimePlatform platform)
    {
        switch (platform)
        {
            case RuntimePlatform.OSXEditor:
                return "OSX";
            case RuntimePlatform.OSXPlayer:
                return "OSX";
            case RuntimePlatform.WindowsPlayer:
                return "Windows";
            case RuntimePlatform.WindowsEditor:
                return "Windows";
            case RuntimePlatform.IPhonePlayer:
                return "Iphone";
            case RuntimePlatform.Android:
                return "Android";
            default:
                return null;
        }
    }
    public static string GetAppFilePath()
    {
        string tmpPath = "";
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            tmpPath = Application.streamingAssetsPath;
        }
        else
        {
            tmpPath = Application.persistentDataPath;
        }
        return tmpPath;
    }
    public static string GetAssetBundlePath()
    {
        string platFolder = GetPlatformFolderName(Application.platform);
        string allPath = Path.Combine(GetAppFilePath(), platFolder);

        return allPath;
    }
    public static string GetRecordFilePath(string SceneName)
    {
        string path = Application.dataPath + "/Art/Scenes/"+SceneName+ "Record.byte";
        return path;
    }

}
