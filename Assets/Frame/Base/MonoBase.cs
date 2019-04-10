using UnityEngine;
using System.Collections;

public class MonoBase : MonoBehaviour {

    public ushort[] msgIds;

    public MonoBase(params ushort[] msgid)
    {
        msgIds = msgid;
    }

    public virtual void RegistEventListen(MonoBase mono,params ushort[] msgids)
    {
        
    }
    public virtual void UnRegistEventListen(MonoBase mono,params ushort[] msgids)
    {

    }
    public virtual void ProcessEvent(MsgBase tmpMsg)
    {

    }
    public virtual void SendMsg(MsgBase tmpMsg)
    {
        MsgCenter.instence.SendMsg(tmpMsg);
    }

}
