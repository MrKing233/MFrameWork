using UnityEngine;
using System.Collections;

public class NetManager:ManangerBase  {


    private static NetManager inst;

    public static NetManager Instance
    {
        get
        {
            if (inst==null)
            {
                inst = new NetManager();
            }
            return inst;
        }
    }
    public void SendMsg(MsgBase msg)
    {
         if (msg.GetMsgType()==(ushort)ManagerID.NetManagerID)
        {
            this.ProcessEvent(msg);
        }
         else
        {
            MsgCenter.instence.SendMsg(msg);
        }
    }

}
