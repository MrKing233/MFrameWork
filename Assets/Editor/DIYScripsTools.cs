using UnityEngine;
using System.IO;
public class DIYScripsTools : UnityEditor.AssetModificationProcessor {


    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        if (path.ToLower().EndsWith(".cs") || path.ToLower().EndsWith(".lua"))
        {
            string content = File.ReadAllText(path);
            content= content.Replace("#CompanyName#", "SH");
            content = content.Replace("#AUTHORNAME#", "JM");
            content = content.Replace("#CreateTime#", System.DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));
            content = content.Replace("#UnityVersion#", Application.unityVersion);
            File.WriteAllText(path, content);
        }
    }
}
