using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ManangerBase 
{
    Dictionary<ushort, NodeBase> eventTree = new Dictionary<ushort, NodeBase>();

    public void RegistEventListen(MonoBase mono, params ushort[] msgIds)
    {
        for (int i = 0; i < msgIds.Length; i++)
        {
            NodeBase node = new NodeBase(mono);
            RegistEventListen(node, msgIds[i]);
        }

    }

    public void RegistEventListen(NodeBase Node, ushort msgid)
    {
        if (eventTree.ContainsKey(msgid))
        {
            NodeBase tmpNode = eventTree[msgid];
            while (tmpNode.next != null)
            {
                tmpNode = tmpNode.next;
            }
            tmpNode.next = Node;
        }
        else
        {
            eventTree.Add(msgid, Node);
        }
    }
    public void UnRegistEventListen(MonoBase mono, params ushort[] msgIds)
    {
        for (int i = 0; i < msgIds.Length; i++)
        {
            if (eventTree.ContainsKey(msgIds[i]))
            {
                UnRegistEventListen(mono, msgIds[i]);
            }
            else
            {
                Debuger.Log("注销的消息不存在 msgid==" + msgIds[i]);
            }

        }

    }
    public void UnRegistEventListen(MonoBase mono, ushort msgId)
    {
        NodeBase tmpNode = eventTree[msgId];
        if (tmpNode != null)
        {
            if (tmpNode.listen = mono)
            {
                if (tmpNode.next != null)
                {
                    eventTree[msgId] = tmpNode.next;
                    tmpNode.next = null;
                    
                }
                else
                {
                    eventTree.Remove(msgId);
                }
            }
            else
            {
                while (tmpNode.next!=null && tmpNode.next.listen!=mono )
                {
                    tmpNode = tmpNode.next;
                }
                if (tmpNode.next==null)
                {
                    Debuger.Log(string.Format("没有对此msgId= {0} 的监听", msgId));
                    return;
                }
                if (tmpNode.next.next!=null)
                {
                    tmpNode.next = tmpNode.next.next;
                    tmpNode.next.next = null;
                }
                else
                {
                    tmpNode.next = null;
                }

            }
        }
    }
    public void ProcessEvent(MsgBase msg)
    {
        if (eventTree.ContainsKey(msg.msgId))
        {
            NodeBase tmpNode = eventTree[msg.msgId];
            do
            {
                tmpNode.listen.ProcessEvent(msg);
                tmpNode = tmpNode.next;
            } while (tmpNode != null);

        }
        else
        {
            Debuger.Log("没有对这个消息的监听 msgid==" + msg.msgId);
        }
    }
}


public class NodeBase
{
    public MonoBase listen;

    public NodeBase next;

    public NodeBase(MonoBase tmpNode)
    {
        listen = tmpNode;
    }
}