using UnityEngine;
using System.Collections;

public class NetBase : MonoBase {

    public override void RegistEventListen(MonoBase mono, params ushort[] msgids)
    {
        NetManager.Instance.RegistEventListen(mono, msgids);
    }

    public override void UnRegistEventListen(MonoBase mono, params ushort[] msgids)
    {
        NetManager.Instance.UnRegistEventListen(mono, msgids);
    }
    public override void SendMsg(MsgBase tmpMsg)
    {
        NetManager.Instance.SendMsg(tmpMsg);
    }
}
