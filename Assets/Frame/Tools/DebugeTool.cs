using UnityEngine;
using System.Text;

public static class Debuger
{
    /// <summary>
    /// 自定义通用打印log
    /// </summary>
    /// <param name="strs"></param>
    public static void Log(params string[] strs)
    {
        StringBuilder sbStr = new StringBuilder();
        for (int i = 0; i < strs.Length; i++)
        {
            sbStr.Append("  ");
            sbStr.Append(strs[i]);
        }
        Debug.Log(sbStr);
    }
    public static void LogError(params string[] strs)
    {
        StringBuilder sbStr = new StringBuilder();
        for (int i = 0; i < strs.Length; i++)
        {
            sbStr.Append("  ");
            sbStr.Append(strs[i]);
        }
        Debug.LogError(sbStr);
    }

    /// <summary>
    /// 字符串拼接
    /// </summary>
    /// <param name="strs"></param>
    public static string StrJoint(params string[] strs)
    {
        StringBuilder sbStr = new StringBuilder();
        for (int i = 0; i < strs.Length; i++)
        {
            sbStr.Append(strs[i]);
        }
        return sbStr.ToString();
    }
}
