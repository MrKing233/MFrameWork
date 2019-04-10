using UnityEngine;
using System.Collections;

public enum UIListenID {
    
    Start_Test=ManagerID.UIManagerID+1,
    LoadUI,
}
public enum AssetListenID
{
    Start=ManagerID.AssetManagerID+1,
    HunkRes,//请求资源
    LoadBundleRes,
    ReleaseBundle,
    ReleaseBundleRes,
}
public enum OtherListId
{
    other=ManagerID.AssetManagerID+1,
}