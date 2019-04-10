using UnityEngine;
using System.Collections;

public class UIBase : MonoBase {

    public override void RegistEventListen(MonoBase mono, params ushort[] msgids)
    {
        UIManager.instance.RegistEventListen(mono, msgids);
    }
    public override void UnRegistEventListen(MonoBase mono, params ushort[] msgids)
    {
        UIManager.instance.UnRegistEventListen(mono, msgids);
    }
    public override void ProcessEvent(MsgBase tmpMsg)
    {
        //UIManager.instance.ProcessEvent(tmpMsg);
    }
    public override void SendMsg(MsgBase tmpMsg)
    {
        UIManager.instance.SendMsg(tmpMsg);
    }

}
