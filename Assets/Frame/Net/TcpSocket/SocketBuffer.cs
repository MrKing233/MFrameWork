//using UnityEngine;
//using System.Collections;
using System;

public delegate void RecvOverEvent(byte[] byteData);
public class SocketBuffer
{

    public byte[] headByte;
    public int headLength = 6;
    public int curRecvLength = 0;
    public byte[] allRecvData;
    public int allDataLength;
    RecvOverEvent recvCallBack;
    public SocketBuffer(int tmpHeadLength, RecvOverEvent recvBack)
    {
        recvCallBack = recvBack;
        headLength = tmpHeadLength;
        headByte = new byte[headLength];
    }

    public void AnalyRecvData(byte[] recvData, int tmpLength)
    {
        if (tmpLength == 0)
        {
            return;
        }
        if (curRecvLength < headLength)
        {
            RecvHead(recvData, tmpLength);
        }
        else
        {
            int tmpallLength = curRecvLength + tmpLength;//已接收+待接收 总长度

            if (tmpallLength>allDataLength) //总长度比需要的一条数据长
            {
                RecvLarger(recvData, tmpLength);
            }
            else if (tmpallLength==allDataLength)
            {
                RecvOneAll(recvData,tmpLength);
            }
            else
            {
                RecvSmall(recvData, tmpLength);
            }


        }
    }
    public void RecvLarger(byte[] recvData, int tmpLength)
    {
        int needLength = allDataLength - curRecvLength;
        Buffer.BlockCopy(recvData, 0, allRecvData, curRecvLength, needLength);
        int curlength = tmpLength - needLength;

        byte[] tmpRecvData = new byte[curlength];

        Buffer.BlockCopy(recvData, curlength, tmpRecvData, 0, curlength);
        recvDataOverCallBack();
        AnalyRecvData(tmpRecvData, curlength);
    }
    public void RecvSmall(byte[] recvData, int tmpLength)
    {
        Buffer.BlockCopy(recvData, 0, allRecvData, curRecvLength, tmpLength);
        curRecvLength += tmpLength;
    }
    public void RecvOneAll(byte[] recvData, int tmpLength)
    {
        Buffer.BlockCopy(recvData, 0, allRecvData, curRecvLength, tmpLength);
        recvDataOverCallBack();
    }
    public void RecvHead(byte[] recvData, int tmpLength)
    {
        int allLength = tmpLength + curRecvLength;
        if (allLength < headLength)
        {
            curRecvLength += tmpLength;
            Buffer.BlockCopy(recvData, 0, headByte, curRecvLength, tmpLength);
            return;
        }
        else
        {
            int headNeedlength = headLength - curRecvLength;

            Buffer.BlockCopy(recvData, 0, headByte, curRecvLength, headNeedlength);
            curRecvLength += headNeedlength;
            allDataLength = BitConverter.ToInt32(headByte, 0);
            allRecvData = new byte[allDataLength];
            Buffer.BlockCopy(recvData, 0, allRecvData, 0, headLength);
            int otherLength = tmpLength - headNeedlength;

            if (allDataLength == headLength)//当总消息长度刚好等于头长度时，说明接收已经完成
            {
                recvDataOverCallBack();

            }

            if (otherLength > 0)
            {
                byte[] tmpbyte = new byte[otherLength];
                Buffer.BlockCopy(recvData, headNeedlength, tmpbyte, 0, otherLength);
                AnalyRecvData(tmpbyte, otherLength);
            }
          

        }

    }
    public void recvDataOverCallBack()
    {
        recvCallBack(allRecvData);
        Reset();
    }
    public void Reset()
    {
        headByte = null;
        curRecvLength = 0;
        allRecvData = null;
        allDataLength = 0;
        recvCallBack = null;
    }

}
