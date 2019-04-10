using UnityEngine;
using System.Collections;

public class NetMsg : MsgBase {
    public byte[] buffer;
    public NetMsg(ushort tmpMsgid)
    {
        this.msgId = tmpMsgid;
    }

}
public class TcpConnectMsg:MsgBase
{
    public string ip;
    public ushort port;
    public TcpConnectMsg(ushort tmpMsgid, string tmpIp,ushort tmpPort)
    {
        this.msgId = tmpMsgid;
        this.ip = tmpIp;
        this.port = tmpPort;
    }

}
