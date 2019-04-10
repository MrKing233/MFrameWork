using UnityEngine;
using System.Collections.Generic;
using MFrameWork.Asset;
public class LoadMananger : MonoBehaviour
{
    public static LoadMananger Instance;
    public Dictionary<string, ABSceneManager> loadManager;
    #region 第一步初始化加载
    private void Awake()
    {
        Instance = this;
        StartCoroutine(LoadABManifest.Instance.StartLoadManifest());
        loadManager = new Dictionary<string, ABSceneManager>();
    }
    #endregion






    #region 第二步加载配置文件
    public void ReadConfiger(string scenceName)
    {
        if (!loadManager.ContainsKey(scenceName))
        {
            ABSceneManager tmpManager = new ABSceneManager(scenceName);
            tmpManager.ReadConfiger();
            loadManager.Add(scenceName, tmpManager);


        }
    }
    #endregion
    public void LoadCallBack(string scencename, string bundleName)//加载回调
    {
        if (loadManager.ContainsKey(scencename))
        {
            ABSceneManager tmpManager = loadManager[scencename];
            StartCoroutine(tmpManager.LoadAssetSys(bundleName));
        }
        else
        {
            Debug.LogError("loadManager.ContainsKey  is not");
        }
    }
    #region 第三步 提供加载功能
    public void LoadAsset(string scencename, string bundleName, LoadingBack loading, LoadEndBack loadend)
    {
        if (!loadManager.ContainsKey(scencename))
        {
            ReadConfiger(scencename);
            Debuger.Log("没有加载配置文件，已自动加载 sceneName== ", scencename, "  bundleName==  ", bundleName);
        }

        ABSceneManager tmpManager = loadManager[scencename];
        tmpManager.InitLoadBundle(bundleName, loading, loadend);

        StartCoroutine(tmpManager.LoadAssetSys(bundleName));

    }
    #endregion
    public string TransForBundleName(string scenceName, string bundleName)
    {
        ABSceneManager tmpManager = loadManager[scenceName];

        if (tmpManager != null)
        {
            return tmpManager.TransForBundleName(bundleName);
        }
        return null;

    }
    public bool IsLoadAssetBundle(string sceneName, string bundleName)
    {
        if (loadManager.ContainsKey(sceneName))
        {
            return loadManager[sceneName].IsLoadAssetBundle(bundleName);
        }
        else
        {
            return false;
        }
    }
    #region  由下层提供
    public Object GetSingleResources(string scenceName, string bundleName, string resName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            ABSceneManager tmpManager = loadManager[scenceName];

            return tmpManager.GetSingleResources(bundleName, resName);
        }
        else
        {
            Debug.LogError(" 获取资源出错 scencename== " + scenceName + "  bundleName== " + bundleName + "  resName== " + resName);
            return null;
        }
    }
    public Object[] GetAllResources(string scenceName, string bundleName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            ABSceneManager tmpManager = loadManager[scenceName];

            return tmpManager.GetAllResources(bundleName);
        }
        else
        {
            Debug.LogError(" 获取资源出错 scencename== " + scenceName + "  bundleName== " + bundleName);
            return null;
        }
    }
    /// <summary>
    /// 释放bundle包内的一个资源
    /// </summary>
    /// <param name="scenceName"></param>
    /// <param name="bunleName"></param>
    /// <param name="res"></param>
    public void DisposeResObj(string scenceName, string bundleName, string res)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            ABSceneManager tmpManager = loadManager[scenceName];

            tmpManager.DisposeResObj(bundleName, res);
        }
        else
        {
            Debug.LogError(" 获取资源出错 scencename== " + scenceName + "  bundleName== " + bundleName + "  resName== " + res);
        }
    }
    /// <summary>
    /// 释放单个bundle包内的所有资源
    /// </summary>
    /// <param name="scenceName"></param>
    /// <param name="bundleName"></param>
    public void DisposeOneBundleRes(string scenceName, string bundleName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            ABSceneManager tmpManager = loadManager[scenceName];

            tmpManager.DisposeOneBundleRes(bundleName);
        }
    }
    /// <summary>
    /// 释放一个场景所有资源
    /// </summary>
    /// <param name="scenceName"></param>
    public void UnLoadAllResObjs(string scenceName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            ABSceneManager tmpManager = loadManager[scenceName];

            tmpManager.DisposeAllBundleRes();
        }
    }

    /// <summary>
    /// 释放一个bundle包
    /// </summary>
    /// <param name="scenceName"></param>
    /// <param name="bundleName"></param>
    public void UnloadAssetBundle(string scenceName, string bundleName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            ABSceneManager tmpManager = loadManager[scenceName];

            tmpManager.DisposeBundle(bundleName);
        }
    }
    /// <summary>
    /// 释放一个场景所有bundle包
    /// </summary>
    public void UnLoadAllAssetBundle(string scenceName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            ABSceneManager tmpManager = loadManager[scenceName];

            tmpManager.DispoaseAllBundle();
            System.GC.Collect();//回收一次GC
        }
    }
    /// <summary>
    /// 释放一个场景所有bundle包和bundle包内资源
    /// </summary>
    /// <param name="scenceName"></param>
    public void UnLoadAllAssetBundleAndRes(string scenceName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            ABSceneManager tmpManager = loadManager[scenceName];

            tmpManager.DisposeAllBundleRes();
            tmpManager.DispoaseAllBundle();
            System.GC.Collect();//回收一次GC
        }
    }
    /// <summary>
    /// 输出一个场景所有bundle内资源名
    /// </summary>
    /// <param name="scenceName"></param>
    public void DebugAllAssetBundle(string scenceName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            ABSceneManager tmpManager = loadManager[scenceName];

            tmpManager.DebugAllBundleAllResName();
        }

    }
    /// <summary>
    /// 输出场景内某个bundle内所有资源名
    /// </summary>
    /// <param name="scenceName"></param>
    /// <param name="bundleName"></param>
    public void DebugAssetBundle(string scenceName, string bundleName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            ABSceneManager tmpManager = loadManager[scenceName];

            tmpManager.DebugBundleAllResName(bundleName);
        }

    }
    #endregion

    //public bool IsLoadBundleFinish(string scenceName, string bundleName)
    //{
    //    bool tmpBool = loadManager.ContainsKey(scenceName);
    //    if (tmpBool)
    //    {
    //        ABSceneManager tmpManager = loadManager[scenceName];
    //        return tmpManager.IsLoadingFinish(bundleName);
    //    }
    //    else
    //    {
    //        Debug.LogError(" is Not Contain scenceName == " + scenceName);
    //        return false;
    //    }
    //}

    //public bool IsLoadingAssetBundle(string scenceName, string bundleName)
    //{
    //    bool tmpBool = loadManager.ContainsKey(scenceName);
    //    if (tmpBool)
    //    {
    //        ABSceneManager tmpManager = loadManager[scenceName];
    //        return tmpManager.IsLoadingAssetBundle(bundleName);
    //    }
    //    else
    //    {
    //        Debug.LogError(" is Not Contain scenceName == " + scenceName);
    //        return false;
    //    }
    //}


    //public bool IsLoadingAssetBundle
    private void OnDestroy()
    {
        loadManager.Clear();
        System.GC.Collect();
    }



}
