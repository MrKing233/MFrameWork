/*  
 *  公司:    SH
 *  作者:    JM
 *  时间:    2019-03-30-11:34:55
 *  版本:    5.4.5p4
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetResNode
{
    public HunkResMsg listen;
    public AssetResNode next;
    public AssetResNode(HunkResMsg msg)
    {
        listen = msg;
    }

}
public class NativeResLoader : MonoBase
{

    void Awake()
    {
        ushort[] msgIds =
        {
            (ushort)AssetListenID.HunkRes,
        };
        RegistEventListen(this, msgIds);
    }
    Dictionary<string, AssetResNode> dicLoadAssetMsg = new Dictionary<string, AssetResNode>();
    public override void ProcessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)AssetListenID.HunkRes://请求资源
                HunkResMsg hunkMsg = (HunkResMsg)tmpMsg;

                break;
            case (ushort)AssetListenID.LoadBundleRes:

                break;
            case (ushort)AssetListenID.ReleaseBundle:

                break;
            case (ushort)AssetListenID.ReleaseBundleRes:

                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 加载完资源回调
    /// </summary>
    /// <param name="bundleName"></param>
    public void LoadendBundleBack(string bundleName)
    {
        if (dicLoadAssetMsg.ContainsKey(bundleName))
        {
            AssetResNode node = dicLoadAssetMsg[bundleName];
           
            do
            {
                CallSengMsgBack(node.listen);
                node = node.next;
            } while (node!=null);
            dicLoadAssetMsg[bundleName] = null;

        }
    }
    public void LoadingBundleBack(string bundleName, float progress)
    {

    }
    public void GetResouce(HunkResMsg msg)
    {
        string transBundleName = LoadMananger.Instance.TransForBundleName(msg.sceneName, msg.bundleName);
        if (LoadMananger.Instance.IsLoadAssetBundle(msg.sceneName, transBundleName))//有加载过bundle
        {
            CallSengMsgBack(msg);

        }
        else
        {
            LoadMananger.Instance.LoadAsset(msg.sceneName, transBundleName, LoadingBundleBack, LoadendBundleBack);

            AddAssetNode(transBundleName, msg);
        }
    }
    public void CallSengMsgBack(HunkResMsg msg)//资源加载完自动回调设定好的回调消息backMsgid
    {
        Object obj= LoadMananger.Instance.GetSingleResources(msg.sceneName, msg.bundleName, msg.resName);
        msg.SetResObj(obj);
        msg.ChangerMsgId();
        SendMsg(msg);
    }
    public void AddAssetNode(string bundleName, HunkResMsg msg)
    {
        if (dicLoadAssetMsg.ContainsKey(bundleName))
        {
            AssetResNode node = dicLoadAssetMsg[bundleName];
            while (node.next != null)
            {
                node = node.next;
            }

            node.next = new AssetResNode(msg); 

        }
        else
        {
            dicLoadAssetMsg.Add(bundleName, new AssetResNode(msg));
        }
    }
    void Start()
    {

    }
    void Update()
    {

    }
}
