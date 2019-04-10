using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System;
public class NetWorkToServer
{
    NetSocket socket;
    private Queue<NetMsg> recvBuffPool;
    private Queue<NetMsg> sendBuffPool;
    Thread SendThread;
    Thread RecvThread;

    #region 连接
    public NetWorkToServer(string ip, ushort port)
    {
        recvBuffPool = new Queue<NetMsg>();
        sendBuffPool = new Queue<NetMsg>();
        socket = new NetSocket();
        socket.AsyConnect(ip, port, AsyConnectCallBack, RecvCallBack);
    }

    public void AsyConnectCallBack(bool isSuccess, SocketState state, string exception)
    {
        if (isSuccess)
        {
            SendThread = new Thread(LoopSend);
            SendThread.Start();
            RecvThread = new Thread(LoopRecv);
            RecvThread.Start();
        }
        else
        {
            Debug.LogError(exception + " socket状态： " + state);
        }
    }
    #endregion
   
    #region 接收
    public void LoopRecv()
    {
        while (socket != null && socket.IsConnected())
        {
            lock (sendBuffPool)
            {
                if (recvBuffPool.Count > 0)
                {

                    //NetManager.Instance.SendMsg()
                }
            }
            Thread.Sleep(3);
        }
    }
    public void RecvCallBack(bool isSuccess, SocketState state, byte[] byteMsg, string exception, string strMessage)
    {
        if (isSuccess)
        {
            NetMsg msg = TransForByteToMsg(byteMsg);
            PutMsgToRecvBuffPool(msg);
        }
        else
        {
            Debug.LogError(exception + " socket状态： " + state);
        }
    }
    /// <summary>
    /// 这里解析只做了解析msgid的操作，具体内容不管。
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public NetMsg TransForByteToMsg(byte[] bytes)//这个转换最好放到NetMsg里作为网络消息的公共转换方法
    {
        int length = BitConverter.ToInt32(bytes, 0);//这里解析的长度包括了消息头
        ushort msgid = (ushort)BitConverter.ToInt32(bytes, 4);
        NetMsg msg = new NetMsg(msgid);
        Buffer.BlockCopy(bytes, 6, msg.buffer, 0, length - 6);
        return msg;
    }
    public void PutMsgToRecvBuffPool(NetMsg msg)
    {
        recvBuffPool.Enqueue(msg);
    }
    #endregion

    #region 发送
    public void LoopSend()
    {
        while (socket != null && socket.IsConnected())
        {
            lock (sendBuffPool)
            {
                if (sendBuffPool.Count > 0)
                {
                    NetMsg msg = sendBuffPool.Dequeue();
                    socket.AsySendMsg(msg.buffer, SendMsgCallBack);
                }
            }
            Thread.Sleep(3);
        }
    }
    public void SendMsgCallBack(bool isSuccess, SocketState state, string exception)
    {
        if (isSuccess)
        {

        }
        else
        {
            Debug.LogError(exception + " socket状态： " + state);
        }
    }
    public void PushMsgToSendBuffPool(NetMsg msg)
    {

    }
    #endregion

    #region 断开

    public void DisConnect()
    {
        if (socket!=null &&socket.IsConnected())
        {
            socket.AsyncDisconnect(DisConnectCallBack);
        }

    }
    public void DisConnectCallBack(bool isSuccess, SocketState state, string exception)
    {
        if (isSuccess)
        {
            SendThread.Abort();
            RecvThread.Abort();
        }
        else
        {
            Debug.LogError(exception+" socket状态： "+ state);
        }
    }
    #endregion
}
