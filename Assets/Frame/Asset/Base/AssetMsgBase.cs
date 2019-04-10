using UnityEngine;
using System.Collections.Generic;
using MFrameWork.Asset;
public class AssetMsgBase : MsgBase {
    public string bundleName;
    public string resName;
    public string sceneName;
}
public class HunkResMsg : AssetMsgBase
{
    //public string sceneName;
    //public string bundleName;
    //public string resName;
    ushort backMsgId;
    Object resObj;
    public HunkResMsg(ushort tmpMsgId,string tmpSceneName,string tmpBundleName,string tmpResName,ushort tmpBackMsgId)
    {
        this.msgId = tmpMsgId;
        this.sceneName = tmpSceneName;
        this.bundleName = tmpSceneName;
        this.resName = tmpSceneName;
        this.backMsgId = tmpBackMsgId;
    }
    public void ChangerMsgId()
    {
        
        this.msgId = backMsgId;
    }
    public void SetResObj(Object obj)
    {
        if (resObj == null)
        {
            resObj = obj;
        }
    }

}
public class LoadBundleResMsg:MsgBase
{
    Dictionary<string, Object> dicObjs;
    public string resName;
    public string bundleName;
    public string sceneName;
    public LoadBundleResMsg(string tmpBundleName, string tmpResName,string tmpSceneName)
    {
        bundleName = tmpBundleName;
        resName = tmpResName;
        sceneName = tmpSceneName;
        dicObjs = new Dictionary<string, Object>();
    }
    public Object this [string resName]
    {
        get
        {
            if (dicObjs[resName]!=null)
            {
                return dicObjs[resName];
            }
            else
            {
                return null;
            }
        }
    }
}
