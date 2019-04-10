using UnityEngine;
using System.Collections;

namespace MFrameWork.Asset
{
    public class LoadABManifest
    {
        public AssetBundleManifest abManifest;
        public AssetBundle manifesetLoader;
        public string manifestPath;

        static LoadABManifest instance;
        public bool isLoadFinish = false;
        public static LoadABManifest Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoadABManifest();
                }
                return instance;
            }
        }
        private LoadABManifest()
        {
            manifestPath = IPathTools.GetAssetBundlePath();

        }
        public IEnumerator StartLoadManifest()
        {
            WWW commonLoad = new WWW(manifestPath);
            if (!string.IsNullOrEmpty(commonLoad.error))
            {
                Debug.LogError("load Manifest error == " + commonLoad.error);
                yield return null;
            }
            if (!commonLoad.isDone)
            {
                yield return commonLoad.progress;
            }
            else
            {
                if (commonLoad.progress >= 1.0f)
                {
                    manifesetLoader = commonLoad.assetBundle;
                    abManifest = manifesetLoader.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                    isLoadFinish = true;
                }
            }
        }

        public string[] GetBundleDependens(string bundleName)
        {
            return abManifest.GetAllDependencies(bundleName);
        }
        public void UnloadManifest()
        {
            manifesetLoader.Unload(true);
        }
    }
}
