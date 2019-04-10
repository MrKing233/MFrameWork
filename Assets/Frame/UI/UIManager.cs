using UnityEngine;
using System.Collections;

public class UIManager :ManangerBase
{
    private static UIManager inst;

    public static UIManager instance
    {
        get
        {
            if (inst == null)
            {
                inst = new UIManager();
            }
            return inst;
        }
    }

    public void SendMsg(MsgBase msg)
    {
        if (msg.GetMsgType()==(ushort)ManagerID.UIManagerID)
        {
            ProcessEvent(msg);
        }
        else
        {
            MsgCenter.instence.SendMsg(msg);
        }
    }


}
public class UIMsg : MsgBase
{
    public UIMsg(ushort tmpMsgId)
    {
        msgId = tmpMsgId;
    }

    public string UIName;
}
