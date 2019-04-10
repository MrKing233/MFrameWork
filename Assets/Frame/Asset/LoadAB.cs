using UnityEngine;
using System.Collections;
namespace MFrameWork.Asset
{
    public delegate void LoadEndBack(string bundleName);
    public delegate void LoadingBack(string bundleName,float progress);
    public class LoadAB
    {
        string bundleName;
        LoadABRes loadABRes;
        string abPath;
        LoadingBack loading;
        LoadEndBack loadend;
        float loadProgress;
        WWW commonLoad;
        AssetBundle bundle;
        public LoadAB(string tmpBundleName, LoadingBack tmpLoading, LoadEndBack tmpLoadend)
        {
            bundleName = tmpBundleName;
            loading = tmpLoading;
            loadend = tmpLoadend;
            SetPathByBundleName(bundleName);
        }
        public void SetPathByBundleName(string bundleName)
        {
           string fullPath= IPathTools.GetAssetBundlePath()+"/"+bundleName;
            abPath = fullPath;
        }
        /// <summary>
        /// 加载一个AB包
        /// </summary>
        /// <returns></returns>
        public IEnumerator CommonLoadAB()
        {
            commonLoad = new WWW(abPath);
            loadProgress = commonLoad.progress;
            if (!commonLoad.isDone)
            {
                if (loading!=null)
                {
                    loading(bundleName, loadProgress);
                }
                yield return loadProgress;
            }
            
            if (loadProgress>=1)
            {
                bundle = commonLoad.assetBundle;
                if (loadABRes==null)
                {
                    loadABRes = new LoadABRes(bundle);
                }
                if (loadend!=null)
                {
                    loadend(bundleName);
                }
                commonLoad = null;
            }
            
        }
        /// <summary>
        /// 加载一个资源
        /// </summary>
        /// <param name="resName"></param>
        /// <returns></returns>
        public Object LoadABResOne(string resName)
        {
            if (loadABRes != null)
            {
                return loadABRes[resName];
            }
            else
            {
                Debug.LogError("loadAB Is Null, resName==" + resName);
                return null;
            }
        }
        /// <summary>
        /// 加载一个资源，及其附属资源
        /// </summary>
        /// <param name="resName"></param>
        /// <returns></returns>
        public Object[] LoadABResAndSub(string resName)
        {
            if (loadABRes != null)
            {
                return loadABRes.LoadResAndSub(resName);
            }
            else
            {
                Debug.LogError("loadAB Is Null, resName==" + resName);
                return null;
            }
        }
        public Object[] LoadAbAllRes()
        {
            if (loadABRes != null)
            {
                return loadABRes.LoadAllRes();
            }
            else
            {
                Debug.LogError("loadAB Is Null, bundleName==  "+bundleName );
                return null;
            }
        }
        /// <summary>
        /// 输出包内所有资源名
        /// </summary>
        public void DebugAllResName()
        {
            if (loadABRes != null)
            {
                loadABRes.DebugAllResName();
            }
            else
            {
                Debug.LogError("loadAB Is Null, bundleName=="+bundleName );
                
            }
        }
        /// <summary>
        /// 仅卸载AB包，无法卸载load出来的资源
        /// </summary>
        public void Dispose()
        {
            if (loadABRes != null)
            {
                loadABRes.UnLoadAssetBundle();
            }
            else
            {
                Debug.LogError("loadAB Is Null, bundleName==" + bundleName);
            }
        }

    }

}