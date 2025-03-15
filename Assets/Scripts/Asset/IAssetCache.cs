using System;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Manager.Assets
{
	public interface IAssetCache
	{
		void CacheAsset<T>(string id, T asset, bool permanent = false);

		UniTask CacheAssetAsync<T>(string id, bool permanent = false) where T : Object;

		bool TryGetCache<T>(string id, out T asset);

		T GetCache<T>(string id);

		bool HasCache(string id);

		void Clear();
	}
}
