using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace MFrameWork.Asset
{
    //public class AssetObj
    //{
    //    public List<Object> objs;
    //    public void ReleaseObj()
    //    {
    //        for (int i = 0; i < objs.Count; i++)
    //        {
    //            Resources.UnloadAsset(objs[i]);
    //        }
    //    }
    //    public AssetObj(params Object[] tmpObj)
    //    {
    //        objs = new List<Object>();
    //        objs.AddRange(tmpObj);
    //    }
    //}

    public class AssetResObj
    {
        Dictionary<string, Object> dicAssetRes;

        string bundleName;
        public AssetResObj()
        {
            dicAssetRes = new Dictionary<string, Object>();
            //dicAssetRes.Add(tmpResName, obj);
        }
        //public AssetResObj(string tmp)
        public void AddRes(string name, Object obj)
        {
            dicAssetRes.Add(name, obj);
        }
        public void ReleaseOneResObj(string resName)
        {
            if (dicAssetRes.ContainsKey(resName))
            {
                Resources.UnloadAsset(dicAssetRes[resName]);
                dicAssetRes[resName] = null;
            }
            else
            {
                Debug.LogError("释放的资源不在缓存中 resName==  " + resName);
            }
        }
        public void ReleaseAllResObj()
        {
            if (dicAssetRes != null)
            {

                List<Object> list = new List<Object>();
                foreach (Object item in dicAssetRes.Values)
                {
                    list.Add(item);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    Resources.UnloadAsset(list[i]);
                }
                list.Clear();
                dicAssetRes.Clear();
            }
        }
        public Dictionary<string,Object> GetAssetDic()
        {
            return dicAssetRes;
        }

    }
    public class LoadABManager
    {
        string scenceName;
        Dictionary<string, ABRelation> loadHelper;//存储一个scence内所有加载过的bundle
        Dictionary<string, AssetResObj> loadAssets;

        public LoadABManager(string tmpScenceName)
        {
            loadHelper = new Dictionary<string, ABRelation>();
            scenceName = tmpScenceName;
        }


        #region 初始化加载一个bundle
        /// <summary>
        /// 初始化加载一个bundle
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="loading"></param>
        /// <param name="loadend"></param>
        public void InitLoadBundle(string bundleName, LoadingBack loading, LoadEndBack loadend)
        {
            if (loadHelper.ContainsKey(bundleName))
            {
                Debuger.Log("bundle包重复初始化 bundleName==  ", bundleName);
            }
            else
            {
                ABRelation abRelation = new ABRelation();
                abRelation.Init(bundleName, loading, loadend);
                loadHelper.Add(bundleName, abRelation);
            }
        }
        #endregion

        #region 协程加载一个bundel，同时加载其依赖的bundle

        public IEnumerator LoadAssetBundleSys(string bundleName)
        {
            while (!LoadABManifest.Instance.isLoadFinish)
            {
                yield return null;
            }
            if (loadHelper != null)
            {
                if (loadHelper.ContainsKey(bundleName))
                {
                    ABRelation abRelation = loadHelper[bundleName];
                    string[] dependens = GetDependences(bundleName);
                    abRelation.SetDependences(dependens);
                    for (int i = 0; i < dependens.Length; i++)
                    {
                        yield return LoadABDependences(dependens[i], bundleName, abRelation.GetProgress(), abRelation.GetEndBackEvent());
                    }
                    yield return abRelation.LoadAssetBundle();
                }
                else
                {
                    Debug.LogError("请先调用InitLoadBundle 初始化 bundle.  bundlename == " + bundleName);
                    yield return null;
                }
            }
            else
            {
                Debug.LogError("loadHelper is null");
                yield return null;
            }
        }

        public IEnumerator LoadABDependences(string bundleName, string refName, LoadingBack loading, LoadEndBack loadend)
        {
            if (!loadHelper.ContainsKey(bundleName))
            {
                InitLoadBundle(bundleName, loading, loadend);
                ABRelation abRelation = loadHelper[bundleName];
                abRelation.AddReferBundles(refName);
                yield return LoadAssetBundleSys(bundleName);
            }
            else
            {
                if (refName != null)
                {
                    ABRelation abRelation = loadHelper[bundleName];
                    abRelation.AddReferBundles(refName);
                    //Debuger.Log("已加载的bundle(可忽略)", bundleName);
                }
            }


        }
        #endregion

        public string[] GetDependences(string bundleName)
        {
            return LoadABManifest.Instance.GetBundleDependens(bundleName);
        }

        #region 加载bundle内的一个资源
        public Object LoadABResOne(string bundleName, string resName)
        {
            if (loadAssets.ContainsKey(bundleName))//先看有没有缓存这个bundle
            {
                AssetResObj tmpRes = loadAssets[bundleName];
                if (tmpRes.GetAssetDic().ContainsKey(resName))//看有没有缓存过这个资源
                {
                    return tmpRes.GetAssetDic()[resName];
                }

            }
            if (loadHelper.ContainsKey(bundleName))//在看有没有加载过这个bundle
            {
                ABRelation abRelation = loadHelper[bundleName];
                Object obj = abRelation.LoadABResOne(resName);
                if (!loadAssets.ContainsKey(bundleName))//如果没有缓存过这个包就new一个新的，并把资源存进去
                {
                    AssetResObj assetRes = new AssetResObj();
                    loadAssets.Add(bundleName, assetRes);
                }
                loadAssets[bundleName].AddRes(resName, obj);
                return obj;
            }
            else
            {
                Debug.LogError("需要先加载这个bundle  bundleName==  " + bundleName);
                return null;
            }

        }
        #endregion

        /// <summary>
        /// 应该慎用这个，如果加载一个已经加载过的资源，会导致重复加载资源
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        public Object[] LoadAbAllRes(string bundleName)
        {
            
            if (loadHelper.ContainsKey(bundleName))//在看有没有加载过这个bundle
            {
                ABRelation abRelation = loadHelper[bundleName];
                Object[] obj = abRelation.LoadABAllRes();
                if (!loadAssets.ContainsKey(bundleName))//如果没有缓存过这个包就new一个新的
                {
                    AssetResObj assetRes = new AssetResObj();
                    loadAssets.Add(bundleName, assetRes);
                }
                for (int i = 0; i < obj.Length; i++)
                {
                    loadAssets[bundleName].AddRes(obj[i].name, obj[i]);
                }

                return obj;
            }
            else
            {
                Debug.LogError("需要先加载这个bundle  bundleName==  " + bundleName);
                return null;
            }
        }

        #region 释放单个资源
        /// <summary>
        /// 释放一个资源
        /// </summary>
        public void DisposeResObj(string bundleName, string resName)
        {
            if (loadAssets.ContainsKey(bundleName))
            {
                AssetResObj assetRes = loadAssets[bundleName];
                assetRes.ReleaseOneResObj(resName);
            }
            else
            {
                Debug.LogError("缓存资源中没有这个bundle   bundleName==  " + bundleName);
            }
        }
        #endregion

        #region 释放一个bundle内所有资源
        /// <summary>
        /// 释放一个bundle内所有加载出的资源
        /// </summary>
        public void DisposeOneBundleRes(string bundleName)
        {
            if (loadAssets.ContainsKey(bundleName))
            {
                AssetResObj assetResObj = loadAssets[bundleName];
                assetResObj.ReleaseAllResObj();
            }
            Resources.UnloadUnusedAssets();
        }
        #endregion

        #region 释放场景内所有bundle内资源
        /// <summary>
        /// 释放一个场景内所有bundle加载出的资源
        /// </summary>
        public void DisposeAllBundleRes()
        {
            List<string> keys = new List<string>();
            keys.AddRange(loadAssets.Keys);

            for (int i = 0; i < keys.Count; i++)
            {
                loadAssets[keys[i]].ReleaseAllResObj();
            }
            Resources.UnloadUnusedAssets();
        }
        #endregion

        #region 卸载bundle包，同时删除对应依赖关系
        /// <summary>
        /// 卸载一个bundle包。这里会根据被依赖关系决定是否能释放这个bundle，所以调用后可能存在这个bundle实际没卸载的情况
        /// </summary>
        /// <param name="bundleName"></param>
        public void DisposeBundle(string bundleName)
        {
            if (loadHelper.ContainsKey(bundleName))
            {
                ABRelation abRelation = loadHelper[bundleName];
                List<string> dependences = abRelation.GetDependences();
                for (int i = 0; i < dependences.Count; i++)
                {
                    ABRelation tmpRelation = loadHelper[dependences[i]];
                    if (tmpRelation != null)
                    {
                        tmpRelation.RemoveReferBundle(bundleName);
                        if (tmpRelation.GetReferBundles().Count <= 0)
                        {
                            DisposeBundle(bundleName);
                        }
                    }

                }
                if (abRelation.GetReferBundles().Count <= 0)
                {
                    abRelation.Dispose();
                    loadHelper[bundleName] = null;
                }

            }
            else
            {
                Debug.LogError("尝试卸载一个没有加载过的bundle  bundleName==  " + bundleName);
            }

        }

        #endregion

        #region 释放所有bundle包

        public void DispoaseAllBundle()
        {
            List<string> keys = new List<string>();
            keys.AddRange(loadHelper.Keys);
            for (int i = 0; i < keys.Count; i++)
            {

                ABRelation loader = loadHelper[keys[i]];

                loader.Dispose();
            }

            loadHelper.Clear();
        }
        #endregion;

        public bool IsLoadAssetBundle(string bundleName)
        {
            if (loadHelper.ContainsKey(bundleName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void DebugBundleAllResName(string bundleName)
        {
            if (loadHelper.ContainsKey(bundleName))
            {
                ABRelation abRela = loadHelper[bundleName];
                abRela.DebugAllResName();
            }
        }
        public void DebugAllBundleAllResName()
        {
            List<string> keys = new List<string>();
            keys.AddRange(loadHelper.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                DebugBundleAllResName(keys[i]);
            }


        }

    }
}