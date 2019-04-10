using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Collections;

namespace MFrameWork.Asset
{
    public class ABSceneManager
    {
        private Dictionary<string, string> dicBundleName;
        LoadABManager abManager;
        string sceneName;
        public ABSceneManager(string tmpSceneName)
        {
            dicBundleName = new Dictionary<string, string>();
            sceneName = tmpSceneName;
            abManager = new LoadABManager(sceneName);
        }
        #region  读表加载一个scene内对应的bundle资源
        /// <summary>
        /// 读表加载bundle，这里的路径有问题
        /// </summary>
        /// <param name="sceneName"></param>
        public void ReadConfiger()
        {
            //string textFileName = "Record.byte";
            string path = IPathTools.GetRecordFilePath(sceneName);

            ReadConfigerData(path);
        }
        public void ReadConfigerData(string path)
        {
            FileInfo file = new FileInfo(path);
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string tmpStr = "";

            do
            {
                tmpStr = sr.ReadLine();
                if (tmpStr != null)
                {
                    string[] tmpArr = tmpStr.Split(new string[] { "  " }, StringSplitOptions.RemoveEmptyEntries);
                    dicBundleName.Add(tmpArr[0], tmpArr[1]);
                    Debuger.Log(tmpStr, tmpArr[0], " ---->> ", tmpArr[1]);
                }
            } while (tmpStr != null);


            sr.Close();
            fs.Close();

        }
        #endregion
        public void InitLoadBundle(string bundleName, LoadingBack loading, LoadEndBack loadend)
        {
            //string tmpBundleName = dicBundleName[bundleName];//这里传进来的bundleName需要转化
            abManager.InitLoadBundle(bundleName, loading, loadend);
        }
        public IEnumerator LoadAssetSys( string bundleName)
        {

            //string tmpBundleName = dicBundleName[bundleName];//这里传进来的bundleName需要转化

            yield return abManager.LoadAssetBundleSys(bundleName);
        }
        public string TransForBundleName(string bundleName)
        {
            if (dicBundleName.ContainsKey(bundleName))
            {
                return dicBundleName[bundleName];
            }
            else
            {
                Debug.LogError("bundleName转换错误，表中没有包含的bundlename或没有初始化表格  bundleName==  " + bundleName);
                return null;
            }
        }
        public bool IsLoadAssetBundle(string bundleName)
        {
            return abManager.IsLoadAssetBundle(bundleName);
        }
        public UnityEngine.Object GetSingleResources(string bundleName,string resName)
        {
            if (abManager!=null)
            {
                return abManager.LoadABResOne(bundleName, resName);
            }
            else
            {
                Debug.LogError("abManager is null bundleName== "+bundleName);
                return null;
            }
        }
        /// <summary>
        /// 应该慎用这个，如果加载一个已经加载过的资源，会导致重复加载资源
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        public UnityEngine.Object[] GetAllResources(string bundleName)
        {
            if (abManager != null)
            {
                return abManager.LoadAbAllRes(bundleName);
            }
            else
            {
                Debug.LogError("ab Mananger Is null  scnenName==  " + sceneName);
                return null;
            }
        }

        /// <summary>
        /// 释放bundle内一个资源
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="resName"></param>
        public void DisposeResObj(string bundleName, string resName)
        {
            if (abManager != null)
            {
                abManager.DisposeResObj(bundleName, resName);
            }
            else
            {
                Debug.LogError("ab Mananger Is null  scnenName==  " + sceneName);
            }
        }
        /// <summary>
        /// 释放单个bundle所有加载的资源
        /// </summary>
        /// <param name="bundleName"></param>
        public void DisposeOneBundleRes(string bundleName)
        {
            if (abManager != null)
            {
                abManager.DisposeOneBundleRes(bundleName);
            }
            else
            {
                Debug.LogError("ab Mananger Is null  scnenName==  " + sceneName);
            }
        }
        /// <summary>
        /// 释放所有bundle加载的资源
        /// </summary>
        /// <param name="bundleName"></param>
        public void DisposeAllBundleRes()
        {
            if (abManager != null)
            {
                abManager.DisposeAllBundleRes();
            }
            else
            {
                Debug.LogError("ab Mananger Is null  scnenName==  " + sceneName);
            }
        }
        /// <summary>
        /// 释放一个bundle包
        /// </summary>
        /// <param name="bundleName"></param>
        public void DisposeBundle(string bundleName)
        {
            if (abManager != null)
            {
                abManager.DisposeBundle(bundleName);
            }
            else
            {
                Debug.LogError("ab Mananger Is null  scnenName==  " + sceneName);
            }
        }
        public void DispoaseAllBundle()
        {
            if (abManager != null)
            {
                abManager.DispoaseAllBundle();
            }
            else
            {
                Debug.LogError("ab Mananger Is null  scnenName==  " + sceneName);
            }
        }
        public void DebugBundleAllResName(string bundleName)
        {
            if (abManager != null)
            {
                abManager.DebugBundleAllResName(bundleName);
            }
            else
            {
                Debug.LogError("ab Mananger Is null  scnenName==  " + sceneName);
            }
        }
        public void DebugAllBundleAllResName()
        {
            if (abManager != null)
            {
                abManager.DebugAllBundleAllResName();
            }
            else
            {
                Debug.LogError("ab Mananger Is null  scnenName==  " + sceneName);
            }
        }

    }
}