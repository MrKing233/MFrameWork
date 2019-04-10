using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.Threading;

public class NetSocket
{
    public delegate void CallBackNormal(bool isSuccess, SocketState state, string exception);
    public delegate void CallBackRecv(bool isSuccess, SocketState state, byte[] byteMsg, string exception, string strMessage);

    Socket clientSocket;
    SocketBuffer RecvBuff;
    CallBackNormal connectBack;
    CallBackNormal sendBack;
    CallBackNormal disConnectBack;
    CallBackRecv recvBack;
    SocketState socketState;
    byte[] recvData = new byte[1024];
    public NetSocket()
    {
        RecvBuff = new SocketBuffer(6, RecvMsgOverBack);
    }
    public bool IsConnected()
    {
        if (clientSocket != null && clientSocket.Connected)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #region 连接服务器
    public void AsyConnect(string ip, ushort port, CallBackNormal normalBack, CallBackRecv recvOverBack)
    {
        socketState = SocketState.Sucess;
        this.connectBack = normalBack;
        this.recvBack = recvOverBack;
        if (clientSocket == null || !clientSocket.Connected)
        {
            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IAsyncResult connect = clientSocket.BeginConnect(endPoint, ConnectCallBack, clientSocket);
            if (!WriteDot(connect))//这里只要处理连接失败的情况，连接成功的话上面ConnectCallBack会处理
            {
                connectBack(false, socketState, "连接服务器超时");
            }
        }
        else
        {
            connectBack(false, SocketState.ConnectError, "套接字已存在");
        }


    }

    public bool WriteDot(IAsyncResult ar)
    {
        int i = 0;
        while (ar.IsCompleted == false)
        {
            i++;
            if (i >= 20)
            {
                socketState = SocketState.TimeOut;
                return false;
            }
            Thread.Sleep(100);
        }
        return true;
    }

    public void ConnectCallBack(IAsyncResult ar)
    {
        try
        {

            clientSocket.EndConnect(ar);

            if (clientSocket.Connected)
            {

                Debug.Log("成功连接服务器");
                socketState = SocketState.ConnectSucess;
                connectBack(true, socketState, "connect success");
                isCloseConnect = true;
            }
            else
            {
                socketState = SocketState.ConnectUnSucessUnKnow;
                connectBack(false, socketState, " connect error");
            }
        }
        catch (Exception e)
        {
            socketState = SocketState.ConnectError;
            Debug.LogError("连接异常");
            connectBack(false, socketState, e.ToString());
        }
    }
    #endregion


    #region 接收数据
    bool isCloseConnect = false;
    public void AsyReciveData()
    {
        if (isCloseConnect)
        {
            return;
        }
        if (clientSocket != null && clientSocket.Connected)
        {
            IAsyncResult recive = clientSocket.BeginReceive(recvData, 0, recvData.Length, SocketFlags.None, RecvDataCallBack, clientSocket);
            if (!WriteDot(recive))
            {
                //socketState = SocketState.RecvUnSucessUnKnow;
                recvBack(false, SocketState.RecvUnSucessUnKnow, null, "Recive Error", null);
            }

        }
        else
        {
            recvBack(false, SocketState.RecvUnSucessUnKnow, null, "套接字为空", null);
        }
    }

    public void RecvDataCallBack(IAsyncResult ar)
    {
        try
        {
            int length = clientSocket.EndReceive(ar);
            if (!clientSocket.Connected)
            {
                recvBack(false, SocketState.RecvUnSucessUnKnow, null, "连接已断开", null);
                return;
            }
            else
            {
                if (length == 0)
                {
                    return;
                }
                RecvBuff.AnalyRecvData(recvData, length);
            }
        }
        catch (Exception e)
        {

            recvBack(false, SocketState.RecvUnSucessUnKnow, null, e.ToString(), null);
            return;
        }
        AsyReciveData();

    }
    public void RecvMsgOverBack(byte[] bytes)
    {
        recvBack(true, SocketState.ReciveSuccess, bytes, null, " recv success");
    }
    #endregion

    #region 发送数据

    public void AsySendMsg(byte[] senderBuff, CallBackNormal sendCallBack)
    {
        sendBack = sendCallBack;
        if (clientSocket != null && clientSocket.Connected)
        {
            IAsyncResult asysend = clientSocket.BeginSend(senderBuff, 0, senderBuff.Length, SocketFlags.None, SendCallBack, clientSocket);
            if (!WriteDot(asysend))
            {
                sendBack(false, SocketState.SendUnSuccessUnKonw, "发送超时");
            }

        }
        else
        {
            sendBack(false, SocketState.SendUnSuccessUnKonw, "套接字为空");
        }



    }

    public void SendCallBack(IAsyncResult ar)
    {
        try
        {
            if (clientSocket.Connected)
            {
                clientSocket.EndSend(ar);
                sendBack(true, SocketState.SendSucess, "发送成功");
            }

        }
        catch (Exception e)
        {

            sendBack(false, SocketState.SendUnSuccessUnKonw, e.ToString());
        }


    }
    #endregion


    #region 断开连接

    public void AsyncDisconnect(CallBackNormal disConnectCallBack)
    {
        try
        {


            disConnectBack = disConnectCallBack;
            if (clientSocket == null || clientSocket.Connected == false)
            {
                disConnectBack(false, SocketState.DisConnectUnknow, "Error  套接字为空或连接已经断开");
            }
            else
            {
                IAsyncResult asyDisConnect = clientSocket.BeginDisconnect(false, DisConnectCallBack, clientSocket);
                if (!WriteDot(asyDisConnect))
                {
                    disConnectBack(false, SocketState.DisConnectUnknow, "Error 断开连接失败");
                }
            }
        }
        catch(Exception e)
        {
            disConnectBack(false, SocketState.DisConnectUnknow, e.ToString());
        }
    }
    public void DisConnectCallBack(IAsyncResult ar)
    {
        try
        {
            clientSocket.EndDisconnect(ar);
            clientSocket.Close();
            clientSocket = null;
            isCloseConnect = true;
            disConnectBack(true, SocketState.DisConnectSucess, "成功断开连接");
        }
        catch (Exception e)
        {

            disConnectBack(false, SocketState.DisConnectUnknow, e.ToString());
        }

    }

    #endregion
}
public enum SocketState
{
    Sucess = 0,
    ConnectSucess,
    SendSucess,
    TimeOut,
    SocketNull,
    ReciveSuccess,
    SocketUnConnect,
    ConnectUnSucessUnKnow,
    ConnectError,
    DisConnectUnknow,
    DisConnectSucess,
    SendUnSuccessUnKonw,
    RecvUnSucessUnKnow
}
