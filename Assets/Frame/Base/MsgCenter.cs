using UnityEngine;
using System.Collections;

public class MsgCenter  {

    static MsgCenter inst;
    public static MsgCenter instence
    {
        get
        {
            if (inst==null)
            {
                inst = new MsgCenter();
            }
            return inst;
        }
    }

    public void SendMsg(MsgBase msg)
    {
        switch (msg.GetMsgType())
        {
            case (ushort)ManagerID.UIManagerID:
                UIManager.instance.SendMsg(msg);
                break;
            case (ushort)ManagerID.NetManagerID:
                NetManager.Instance.SendMsg(msg);
                break;
            case (ushort)ManagerID.AssetManagerID:
                AssetMananger.Instance.SendMsg(msg);
                break;
            case (ushort)ManagerID.AudioManagerID:
                //UIManager.instance.SendMsg(msg);
                break;
            default:
                break;
        }
    }

}
