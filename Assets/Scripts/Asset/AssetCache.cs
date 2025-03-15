using System;
using System.Collections;
using System.Collections.Generic;
using com.ktgame.assets.loader.core;
using com.ktgame.core;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Object = UnityEngine.Object;

namespace Manager.Assets
{
	[Serializable]
	public class AssetCache : IAssetCache
	{
		[ShowInInspector, ReadOnly] private readonly Dictionary<string, object> _cache;
		private readonly List<string> _tempCacheIds;
		private readonly IAssetLoader _assetLoader;
		private ILogger _logger;

		public AssetCache(IAssetLoader assetLoader, ILogger logger)
		{
			_cache = new Dictionary<string, object>();
			_tempCacheIds = new List<string>();
			_assetLoader = assetLoader;
			_logger = logger;
		}

		public void CacheAsset<T>(string id, T asset, bool permanent = false)
		{
			if (_cache.ContainsKey(id))
			{
				_logger.Error("Duplicate caching id " + id);
				return;
			}

			_cache.Add(id, asset);

			if (!permanent)
			{
				_tempCacheIds.Add(id);
			}
		}

		public async UniTask CacheAssetAsync<T>(string id, bool permanent = false) where T : Object
		{
			if (_cache.ContainsKey(id))
			{
				_logger.Error("Duplicate caching id " + id);
				return;
			}

			var asset = await _assetLoader.LoadAsync<T>(id).Task;

			CacheAsset(id, asset);

			if (!permanent)
			{
				_tempCacheIds.Add(id);
			}
		}

		public bool TryGetCache<T>(string id, out T asset)
		{
			asset = default(T);
			if (!_cache.ContainsKey(id))
			{
				_logger.Error("Duplicate caching id " + id);
				return false;
			}

			var cachedAsset = _cache[id];
			var cachedType = cachedAsset.GetType();
			var castType = typeof(T);

			if (cachedType.IsInstanceOfType(castType))
			{
				_logger.Error("Cached asset id " + id + " expects type of " + cachedAsset.GetType() + " but being casted to " + typeof(T));
				return false;
			}

			asset = (T)_cache[id];
			return true;
		}

		public T GetCache<T>(string id)
		{
			var asset = default(T);
			if (!_cache.ContainsKey(id))
			{
				_logger.Error("Duplicate caching id " + id);
				return asset;
			}

			var cachedAsset = _cache[id];

			if (cachedAsset == null)
			{
				_logger.Error("Cached asset does not exist " + id);
				return asset;
			}

			var cachedType = cachedAsset.GetType();
			var castType = typeof(T);

			if (cachedType.IsInstanceOfType(castType))
			{
				_logger.Error("Cached asset id " + id + " expects type of " + cachedAsset.GetType() + " but being casted to " + typeof(T));
				return asset;
			}

			asset = (T)_cache[id];
			return asset;
		}

		public bool HasCache(string id)
		{
			return _cache.ContainsKey(id);
		}

		public void Clear()
		{
			foreach (var id in _tempCacheIds)
			{
				_cache.Remove(id);
			}

			_tempCacheIds.Clear();
		}
	}
}
