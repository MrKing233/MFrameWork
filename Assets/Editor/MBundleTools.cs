using UnityEngine;
using System.IO;
using UnityEditor;
using System.Collections.Generic;
using System.Text;
public class MBundleTools : Editor
{
    [MenuItem("iTools/BuildeAssetBudle")]

    public static void BuildAssetBundle()
    {
        //streamingassetpath/Windows....
        string outPath = IPathTools.GetAssetBundlePath(); // Application.dataPath + "/AssetBundles";

        DirectoryInfo dir = new DirectoryInfo(outPath);
        if (!dir.Exists)
        {
            dir = Directory.CreateDirectory(outPath);
            Debug.Log("路径不存在，已创建 == " + outPath);
        }

        BuildPipeline.BuildAssetBundles(outPath, 0, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }
    [MenuItem("iTools/ClearAllAssetBundle")]
    public static void ClearAllBundle()
    {
        string ClearPath = IPathTools.GetAssetBundlePath();
        LoopDeleteFile(ClearPath);

        for (int i = 0; i < strStack.Count; i++)
        {
            Directory.Delete(strStack.Pop());
        }

        AssetDatabase.Refresh();

    }
    private static Stack<string> strStack = new Stack<string>();
    public static void LoopDeleteFile(string ClearPath)
    {
        if (Directory.Exists(ClearPath))
        {
            DirectoryInfo dir = new DirectoryInfo(ClearPath);
            FileSystemInfo[] info = dir.GetFileSystemInfos();

            for (int i = 0; i < info.Length; i++)
            {
                FileInfo file = info[i] as FileInfo;
                if (file != null)
                {
                    file.Delete();

                }
                else
                {
                    LoopDeleteFile(info[i].FullName);
                    strStack.Push(info[i].FullName);
                }

            }

        }
    }
    [MenuItem("iTools/MarkAssetBundleName")]
    public static void MarkAssetBundle()
    {
        AssetDatabase.RemoveUnusedAssetBundleNames();
        string path = Application.dataPath + "/Art/Scenes/";

        DirectoryInfo dir = new DirectoryInfo(path);
        FileSystemInfo[] fileInfos = dir.GetFileSystemInfos();

        for (int i = 0; i < fileInfos.Length; i++)
        {
            FileSystemInfo info = fileInfos[i];
            if (info is DirectoryInfo)
            {
                string tmpPath = Path.Combine(path, info.Name);
                ScenceOverView(tmpPath);
            }

        }
        AssetDatabase.Refresh();

    }
    public static void ScenceOverView(string scencePath)
    {
        string textFileName = "Record.byte";
        string tmpPath = scencePath + textFileName;

        Dictionary<string, string> readDict = new Dictionary<string, string>();
        ChangeHead(scencePath, readDict);
        FileStream fs = new FileStream(tmpPath, FileMode.OpenOrCreate);
        //StreamWriter sw = new StreamWriter(fs);
        //BinaryReader br = new BinaryReader(fs);
        BinaryWriter bw = new BinaryWriter(fs);

        foreach (string key in readDict.Keys)
        {
            Debuger.LogError(key, readDict[key]);
            //sw.Write(key);
            //sw.Write("   ");
            //sw.Write(readDict[key]);
            //sw.Write("\n");
            bw.Write(Encoding.Default.GetBytes(key));
            bw.Write(Encoding.Default.GetBytes("  "));
            bw.Write(Encoding.Default.GetBytes(readDict[key]));
            bw.Write(Encoding.Default.GetBytes("\n"));
        }

        fs.Close();
        //BinaryReader br = new BinaryReader(fs);

        //Debug.LogError(Encoding.Default.GetString( br.ReadBytes(2)));

        bw.Close();


    }
    public static void ChangeHead(string fullPath, Dictionary<string, string> dict)
    {
        int tmpCount = fullPath.IndexOf("Assets");
        int tmpLength = fullPath.Length;

        string replacePath = fullPath.Substring(tmpCount, tmpLength - tmpCount);
        //Debuger.LogError(replacePath);//  Assets/Art/Scenes/ScenesOne
        DirectoryInfo dir = new DirectoryInfo(fullPath);
        ListFiles(dir, replacePath, dict);


    }

    /// <summary>
    /// 循环遍历文件夹内所有文件
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="replacePath"></param>
    /// <param name="dict"></param>
    public static void ListFiles(DirectoryInfo dir, string replacePath, Dictionary<string, string> dict)
    {
        if (dir != null)
        {
            FileSystemInfo[] fileInfos = dir.GetFileSystemInfos();
            for (int i = 0; i < fileInfos.Length; i++)
            {
                if (fileInfos[i] is FileInfo)
                {
                    FileInfo file = fileInfos[i] as FileInfo;
                    ChangerMark(file, replacePath, dict);
                }
                else
                {
                    ListFiles(fileInfos[i] as DirectoryInfo, replacePath, dict);
                }


            }
        }
        else
        {
            Debug.LogError("文件路径不存在 ");
        }

    }
    /// <summary>
    /// 改变标记
    /// </summary>
    /// <param name="tmpFile"></param>
    /// <param name="replacePath"></param>
    /// <param name="dict"></param>
    public static void ChangerMark(FileInfo tmpFile, string replacePath, Dictionary<string, string> dict)
    {
        if (tmpFile.Extension == ".meta")
        {
            return;
        }
        else
        {
            string endPath= GetBundlePath(tmpFile, replacePath);
            Debuger.LogError("bundleName== ", endPath);
            ChangeAssetMark(tmpFile, endPath, dict);
        }
    }
    public static void ChangeAssetMark(FileInfo tmpFile,string endPath,Dictionary<string,string> dict)
    {
        string fullName = tmpFile.FullName;
        int assetCount = fullName.IndexOf("Assets");
        string assetPath = fullName.Substring(assetCount);
        AssetImporter importer = AssetImporter.GetAtPath(assetPath);
        importer.assetBundleName = endPath;
        if (tmpFile.Extension==".unity")//根据具体情况设定bundle后缀
        {
            importer.assetBundleVariant = "u3d";

        }
        else
        {
            importer.assetBundleVariant = "ly";
        }
        string scenceName = "";
        string[] subMark = endPath.Split("/".ToCharArray());
        if (subMark.Length > 1)
        {
            scenceName = subMark[1];
        }
        else
        {
            scenceName = endPath;
        }
        string modlePath = endPath.ToLower() + "." + importer.assetBundleVariant;
        Debuger.Log(modlePath, "   sssssssssssssssssss");
        if (!dict.ContainsKey(scenceName))
        {
            dict.Add(scenceName, modlePath);
        }
    }
    public static string GetBundlePath(FileInfo file, string replacePath)
    {
        string fullName = file.FullName;
        Debuger.Log("first = ", fullName);
        fullName = FixedWindowsPath(fullName);
        Debuger.Log("scend = ", fullName);
    
        int assetCount = fullName.IndexOf(replacePath);
        //assetCount += replacePath.Length + 1;
        int nameCount = fullName.LastIndexOf(file.Name);
        string noNamePath= fullName.Substring(0,nameCount-1);//除了名字外还要去掉一个"/"，所以-1
        string lastStr= noNamePath.Substring(assetCount + replacePath.Length);
        int tmpCount = replacePath.LastIndexOf("/");
        string scenceHead = replacePath.Substring(tmpCount + 1);
        string endPath = scenceHead + lastStr;
        Debug.LogError(endPath);
        return endPath;

    }
    public static string FixedWindowsPath(string tmpPath)
    {

        tmpPath = tmpPath.Replace("\\", "/");
        return tmpPath;
    }
    //public static void ChangeAssetMark(string tmpFile,string)

}
