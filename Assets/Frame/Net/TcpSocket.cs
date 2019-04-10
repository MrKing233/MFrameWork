using UnityEngine;
using System.Collections;

public class TcpSocket : NetBase {
    NetWorkToServer tcpSocket;
    private void Awake()
    {
        ushort[] msgIds =
        {
            (ushort)NetListenID.TcpConnect,
            (ushort)NetListenID.TcpSendMsg,
            (ushort)UIListenID.Start_Test,
        };
        RegistEventListen(this, msgIds);
    }


    public override void ProcessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)NetListenID.TcpConnect:
                TcpConnectMsg msg = (TcpConnectMsg)tmpMsg;
                if (tcpSocket==null)
                {
                    tcpSocket = new NetWorkToServer(msg.ip, msg.port);
                }
                break;

            case (ushort)NetListenID.TcpSendMsg:
                if (tcpSocket != null)
                {
                    tcpSocket.PushMsgToSendBuffPool((NetMsg)tmpMsg);
                }
                else
                {
                    Debug.LogError("tcpSocket  is null");
                }
                Debug.LogError("tcpSocket  收到TcpSendMsg的消息，");
                break;
            default:
                break;
        }
    }

}
