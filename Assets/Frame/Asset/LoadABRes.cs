using UnityEngine;
namespace MFrameWork.Asset
{
    public class LoadABRes
    {
        AssetBundle bundle;
        public LoadABRes(AssetBundle tmpBundle)
        {
            bundle = tmpBundle;
        }
        /// <summary>
        /// 加载AB包单个资源
        /// </summary>
        /// <param name="resName"></param>
        /// <returns></returns>
        public Object this[string resName]
        {
            get
            {
                if (bundle == null || !bundle.Contains(resName))
                {
                    Debuger.Log("bundle is null ==", bundle.name);
                    return null;
                }
                return bundle.LoadAsset(resName);
            }
        }
        /// <summary>
        /// 加载AB包单个资源及其附属资源
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        public Object[] LoadResAndSub(string resName)
        {
            if (bundle == null || !bundle.Contains(resName))
            {
                Debuger.Log("bundle is null ==", bundle.name);
                return null;
            }
            else
            {
                return bundle.LoadAssetWithSubAssets(resName);
            }
        }
        /// <summary>
        /// 加载ab包所有资源
        /// </summary>
        /// <returns></returns>
        public Object[] LoadAllRes()
        {
            if (bundle == null)
            {
                Debuger.Log("bundle is null ==");
                return null;
            }
            else
            {
                return bundle.LoadAllAssets();
            }
        }
        /// <summary>
        /// 查看ab包所有资源名
        /// </summary>
        public void DebugAllResName()
        {
            if (bundle == null)
            {
                Debuger.Log("bundle is null ==", bundle.name);
            }
            else
            {
                string[] ResName = bundle.GetAllAssetNames();
                for (int i = 0; i < ResName.Length; i++)
                {
                    Debug.Log(string.Format("BundleName=={0}  第{1}个ResName=={2}", bundle.name, i, ResName[i]));
                }
            }
        }
        /// <summary>
        /// 仅提供释放AB包功能，不能卸载load的资源
        /// </summary>
        public void UnLoadAssetBundle()
        {
            if (bundle == null)
            {
                Debuger.Log("bundle is null ==", bundle.name);
            }
            else
            {
                bundle.Unload(false);
            }
        }
    }
}
