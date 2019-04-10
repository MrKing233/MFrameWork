using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace MFrameWork.Asset
{
    public class ABRelation
    {
        List<string> ABDependences;//这个bundle依赖的关系
        List<string> ReferBundles;//依赖于这个bundle的关系

        LoadAB loadAB;
        LoadingBack loadingBack;
        LoadEndBack loadendBack;
        string bundleName;
        bool isLoadFinish=false;
        public ABRelation()
        {
            ABDependences = new List<string>();
            ReferBundles = new List<string>();
        }
        public void LoadFinishBack(string bundleName)
        {
            isLoadFinish = true;
            loadendBack(bundleName);
        }
        public bool IsLoadFinish()
        {
            return isLoadFinish;
        }
        public void Init(string tmpBundleName,LoadingBack tmpLoading,LoadEndBack tmpLoadend)
        {
            bundleName = tmpBundleName;
            loadingBack = tmpLoading;
            loadendBack = tmpLoadend;
            if (loadAB == null)
            {
                loadAB = new LoadAB(tmpBundleName,loadingBack,LoadFinishBack);
            }
        }
        #region 加载bundle包
        public IEnumerator LoadAssetBundle()
        {
            if (loadAB != null)
            {
                return loadAB.CommonLoadAB();
            }
            else
            {
                Debug.LogError("LoadAB is null  bundleName== "+bundleName);
                return null;
            }
        }
        #endregion

        #region 加载一个资源
        public Object LoadABResOne(string tmpResName)
        {
            if (loadAB!=null)
            {
                return loadAB.LoadABResOne(tmpResName);
            
            }
            else
            {
                Debug.LogError("loadAB is null bundleName== " + bundleName);
                return null;
            }
        }
        #endregion

        #region 加载一个资源，及其附属资源
        public Object[] LoadABResAndSub(string tmpResName)
        {

            if (loadAB != null)
            {
                return loadAB.LoadABResAndSub(tmpResName);

            }
            else
            {
                Debug.LogError("loadAB is null bundleName== " + bundleName);
                return null;
            }
        }
        #endregion
        public Object[] LoadABAllRes()
        {
            if (loadAB != null)
            {
                return loadAB.LoadAbAllRes();

            }
            else
            {
                Debug.LogError("loadAB is null bundleName== " + bundleName);
                return null;
            }
        }

        #region 输出bundle所有资源名
        public void DebugAllResName()
        {
            if (loadAB != null)
            {
                loadAB.DebugAllResName();

            }
            else
            {
                Debug.LogError("loadAB is null bundleName== " + bundleName);
            }
        }
        #endregion

        #region 卸载bundle
        /// <summary>
        /// 卸载一个bundle
        /// </summary>
        public void Dispose()
        {
            if (loadAB != null)
            {
                loadAB.Dispose();

            }
            else
            {
                Debug.LogError("loadAB is null bundleName== " + bundleName);
            }
        }
        #endregion

        #region bundle的依赖和被依赖关系管理
        public void SetDependences(string[] tmpDependens)
        {
            ABDependences.AddRange(tmpDependens);
        }
        public List<string> GetDependences()
        {
            return ABDependences;
        }
        public LoadingBack GetProgress()
        {
            return loadingBack;
        }
        public LoadEndBack GetEndBackEvent()
        {
            return loadendBack;
        }
        public void AddReferBundles(string tmpReferBundles)
        {
            if (!ReferBundles.Contains(tmpReferBundles))
            {
                ReferBundles.Add(tmpReferBundles);
            }
        }
        public List<string> GetReferBundles()
        {
            return ReferBundles;
        }
        public void RemoveDependen(string removeName)
        {
            if (ABDependences!=null)
            {
                for (int i = 0; i < ABDependences.Count; i++)
                {
                    if (ABDependences[i].Equals(removeName))
                    {
                        ABDependences.RemoveAt(i);
                    }
                }
            }
        }
        public void RemoveReferBundle(string removeName)
        {
            if (ReferBundles!=null)
            {
                for (int i = 0; i < ReferBundles.Count; i++)
                {
                    if (ReferBundles[i].Equals(removeName))
                    {
                        ReferBundles.RemoveAt(i);
                    }
                }
            }
        }
        #endregion


        #region 输出依赖关系和被依赖关系
        public void ShowAllDependens()
        {
            Debuger.Log(bundleName, "依赖的关系 ：---------->>>");
            for (int i = 0; i < ABDependences.Count; i++)
            {
                Debug.Log(ABDependences[i]);
            }
        }
        public void ShowAllReferBundles()
        {
            Debuger.Log(bundleName, "被依赖关系：---------->>>");
            for (int i = 0; i < ReferBundles.Count; i++)
            {
                Debug.Log(ReferBundles[i]);
            }
        }

        #endregion
    }
}
