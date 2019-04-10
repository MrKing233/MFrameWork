using UnityEngine;
using System.Collections;

public class test111 : UIBase {

    // Use this for initialization
    private void Awake()
    {
        ushort[] msgIds =
        {
            (ushort)UIListenID.Start_Test,
            (ushort)OtherListId.other,
        };
        RegistEventListen(this, msgIds);
    }
    void Start () {
        Debuger.Log("5455555", "adsf","t1232131");

	}
    public override void ProcessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)UIListenID.Start_Test:
                Debuger.Log("Get Message  UIListenID.Start_Test",((UIMsg)tmpMsg).UIName);
                break;
            default:
                break;
        }
    }
    // Update is called once per frame

    void Update () {
	
	}
}
