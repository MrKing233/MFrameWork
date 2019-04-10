using UnityEngine;
using System.Collections;

public class MsgBase  {

    public ushort msgId;

    public MsgBase (ushort msgid)
    {
        msgId = msgid;
    }
    public MsgBase ()
    {
        msgId = 0;
    }
    public ushort GetMsgType()
    {
        int id = msgId / FrameTools.Multi;
        int ManagerIDType = id * FrameTools.Multi;
        return (ushort)ManagerIDType;
    }

}
