using UnityEngine;
using System.Collections;
/// <summary>
/// Asset资源管理类
/// </summary>
public class AssetMananger :ManangerBase {

    private static AssetMananger insten;

    public static AssetMananger Instance
    {
        get
        {
            if (insten==null)
            {
                insten = new AssetMananger();
            }
            return insten;
        }
    }
    public void SendMsg(MsgBase tmpMsg)
    {
        if (tmpMsg.GetMsgType()==(ushort)ManagerID.AssetManagerID)
        {
            Instance.ProcessEvent(tmpMsg);
        }
        else
        {
            MsgCenter.instence.SendMsg(tmpMsg);
        }

    }
	
}
