using UnityEngine;
using System.Collections;


public class FrameTools  {

    public const ushort Multi = 5000;
}

public enum ManagerID {
    AssetManagerID = 0,
    NetManagerID =FrameTools.Multi*1,
    GameManagerID=FrameTools.Multi*2,
    UIManagerID=FrameTools.Multi*3,
    AudioManagerID=FrameTools.Multi*4,
    NPCManangerID=FrameTools.Multi*5,

}
