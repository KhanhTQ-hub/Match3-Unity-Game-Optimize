using System;
using System.Collections.Generic;
using System.Linq;
using com.ktgame.assets.loader.addressables;
using com.ktgame.assets.loader.core;
using com.ktgame.assets.loader.resources;
using com.ktgame.core.di;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;
using ILogger = com.ktgame.core.ILogger;
using Object = UnityEngine.Object;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine.AddressableAssets;
#endif

namespace Manager.Assets
{
	public class AssetManager : MonoBehaviour, IAssetManager
	{
		[SerializeField] private ResourcesAssetLoaderObject _resourceAssetLoader;
		[SerializeField] private AddressableAssetLoaderObject _addressableAssetLoader;
		[SerializeField] private PreloadedAssetLoaderObject _preloadedAssetLoader;
		[SerializeField] private SpriteAtlasSet[] _spriteAtlasSets;

		public int Priority => 0;

		public bool IsInitialized { get; private set; }

		[Inject] private readonly ILogger _logger;

		[ShowInInspector, ReadOnly] private IAssetCache _assetCache;
		private SpriteAtlasSet _spriteAtlasSet;

		public UniTask Initialize()
		{
			if (_resourceAssetLoader == null)
			{
				throw new ArgumentNullException(nameof(ResourcesAssetLoaderObject));
			}

			if (_addressableAssetLoader == null)
			{
				throw new ArgumentNullException(nameof(AddressableAssetLoaderObject));
			}

			if (_preloadedAssetLoader == null)
			{
				throw new ArgumentNullException(nameof(PreloadedAssetLoaderObject));
			}
			
			_spriteAtlasSet = _spriteAtlasSets[0];
			_assetCache = new AssetCache(_addressableAssetLoader, _logger);
			_spriteAtlasSet.InitLookup(_addressableAssetLoader);
			SpriteAtlasManager.atlasRequested += OnAtlasRequest;

			IsInitialized = true;
			return UniTask.CompletedTask;
		}

		private void OnAtlasRequest(string atlasTag, Action<SpriteAtlas> action)
		{
			if (!_spriteAtlasSet.HasAtlasLoader(atlasTag))
			{
				_logger.Error("There is no SpriteAtlasLoader of tag: " + atlasTag);
				action.Invoke(null);
				return;
			}

			var spriteAtlasLoader = _spriteAtlasSet.GetSpriteLoader(atlasTag);
			var atlas = spriteAtlasLoader.Atlas;
			spriteAtlasLoader.IncreaseRefCount();
			action.Invoke(atlas);
		}

		// public Sprite GetCommonIcon(string icon)
		// {
		// 	if (icon != null)
		// 	{
		// 		var sprite = GetIcon(icon, AtlasTag.Common, false);
		// 		if (sprite != null)
		// 		{
		// 			return sprite;
		// 		}
		// 	}
		//
		// 	return null;
		// }
		//
		// public Sprite GetMixingIcon(string icon)
		// {
		// 	if (icon != null)
		// 	{
		// 		var sprite = GetIcon(icon, AtlasTag.IconMixing, false);
		// 		if (sprite != null)
		// 		{
		// 			return sprite;
		// 		}
		// 	}
		//
		// 	return null;
		// }
		//
		// public Sprite GetMaterialIcon(string icon)
		// {
		// 	if (icon != null)
		// 	{
		// 		var sprite = GetIcon(icon, AtlasTag.IconMaterial, false);
		// 		if (sprite != null)
		// 		{
		// 			return sprite;
		// 		}
		// 	}
		//
		// 	return null;
		// }
		//
		// public Sprite GetMaterialMiningIcon(string icon)
		// {
		// 	if (icon != null)
		// 	{
		// 		var sprite = GetIcon(icon, AtlasTag.IconMaterialMining, false);
		// 		if (sprite != null)
		// 		{
		// 			return sprite;
		// 		}
		// 	}
		//
		// 	return null;
		// }
		//
		// public Sprite GetBannerEvent(string eventName)
		// {
		// 	if (eventName != null)
		// 	{
		// 		var sprite = GetIcon(eventName, AtlasTag.BannerEvent, false);
		// 		if (sprite != null)
		// 		{
		// 			return sprite;
		// 		}
		// 	}
		//
		// 	return null;
		// }
		//
		// public Sprite GetHeroIcon(string icon)
		// {
		// 	if (icon != null)
		// 	{
		// 		var sprite = GetIcon(icon, AtlasTag.IconHero, false);
		// 		if (sprite != null)
		// 		{
		// 			return sprite;
		// 		}
		// 	}
		//
		// 	return null;
		// }

		public SpriteAtlasLoader GetSpriteAtlasLoader(string atlasTag)
		{
			if (!_spriteAtlasSet.HasAtlasLoader(atlasTag))
			{
				_logger.Error("There is no SpriteAtlasLoader of tag: " + atlasTag);
				return null;
			}

			return _spriteAtlasSet.GetSpriteLoader(atlasTag);
		}

		public async UniTask<TAsset> GetAsset<TAsset>(string key) where TAsset : Object
		{
			TAsset asset;
			if (_assetCache.HasCache(key))
			{
				asset = _assetCache.GetCache<TAsset>(key);
			}
			else
			{
				asset = await _addressableAssetLoader.LoadAsync<TAsset>(key).Task;
				_assetCache.CacheAsset(key, asset);
			}

			return asset;
		}

		public AssetRequest<TAsset> Preloaded<TAsset>(string address) where TAsset : Object
		{
			return _preloadedAssetLoader.Load<TAsset>(address);
		}

		public AssetRequest<TAsset> PreloadedAsync<TAsset>(string address) where TAsset : Object
		{
			return _preloadedAssetLoader.LoadAsync<TAsset>(address);
		}

		public AssetRequest<TAsset> ResourceLoad<TAsset>(string address) where TAsset : Object
		{
			return _resourceAssetLoader.Load<TAsset>(address);
		}

		public AssetRequest<TAsset> ResourceLoadAsync<TAsset>(string address) where TAsset : Object
		{
			return _resourceAssetLoader.LoadAsync<TAsset>(address);
		}

		public AssetRequest<TAsset> AddressableLoad<TAsset>(string address) where TAsset : Object
		{
			return _addressableAssetLoader.Load<TAsset>(address);
		}

		public AssetRequest<Object> AddressableLoad(string address)
		{
			return _addressableAssetLoader.Load(address);
		}

		public AssetRequest<TAsset> AddressableLoadAsync<TAsset>(string address) where TAsset : Object
		{
			return _addressableAssetLoader.LoadAsync<TAsset>(address);
		}

		public void AddressableRelease(AssetRequest request)
		{
			_addressableAssetLoader.Release(request);
		}

		public void Dispose()
		{
			foreach (var spriteAtlasSet in _spriteAtlasSets)
			{
				spriteAtlasSet.ReleaseUnusedAtlases();
			}

			_resourceAssetLoader = null;
			_addressableAssetLoader = null;
			_preloadedAssetLoader = null;
			_assetCache.Clear();
		}

		private Sprite GetIcon(string key, string atlasTag, bool permanentCache)
		{
			if (!_spriteAtlasSet.HasAtlasLoader(atlasTag))
			{
				return null;
			}

			var id = $"{atlasTag}/{key}";
			var loader = _spriteAtlasSet.GetSpriteLoader(atlasTag);
			var atlas = loader.Atlas;
			Sprite icon;

			if (_assetCache.HasCache(id))
			{
				icon = _assetCache.GetCache<Sprite>(id);
			}
			else
			{
				icon = atlas.GetSprite(key);
				if (icon != null)
				{
					loader.IncreaseRefCount();
					_assetCache.CacheAsset(id, icon, permanentCache);
				}
			}

			return icon;
		}

#if UNITY_EDITOR
		[Button, GUIColor("cyan")]
		private async void CreateAtlasLoaders()
		{
			var tags = new List<string>();
			foreach (var spriteAtlasSet in _spriteAtlasSets)
			{
				var classType = string.Empty;

				var atlases = LoadSpriteAtlases(classType);
				var loaders = new List<SpriteAtlasLoader>(atlases.Length);
				foreach (var atlas in atlases)
				{
					var atlasTag = atlas.tag;
					//var path = $"{classType}/{atlas.name}";
					var atlasPath = $"{atlas.name}";

					if (!tags.Contains(atlasTag))
					{
						tags.Add(atlasTag);
					}

					loaders.Add(new SpriteAtlasLoader(atlasTag, atlasPath));
				}

				spriteAtlasSet.AtlasLoaders = loaders.ToArray();
			}

			await using (var writer = new StreamWriter("Assets/Scripts/Generated/AtlasTag.cs"))
			{
				await writer.WriteLineAsync("// This file is auto-generated. Do not modify.");
				await writer.WriteLineAsync("public static class AtlasTag");
				await writer.WriteLineAsync("{");

				foreach (var t in tags)
				{
					var key = t.Replace("/", "_").Replace("-", "_");
					await writer.WriteLineAsync($"	public const string {key} = \"{t}\";");
				}

				await writer.WriteLineAsync("}");
			}

			AssetDatabase.Refresh();
			Debug.Log("AtlasTag.cs has been generated successfully.");
		}

		private static SpriteAtlas[] LoadSpriteAtlases(string subFolder)
		{
			var findAssets = AssetDatabase.FindAssets($"t: {nameof(SpriteAtlas)}", new[] { $"Assets/SpriteAtlas/{subFolder}" });
			if (findAssets.Length == 0)
			{
				return Array.Empty<SpriteAtlas>();
			}

			return findAssets
				.Select(AssetDatabase.GUIDToAssetPath)
				.Select(AssetDatabase.LoadAssetAtPath<SpriteAtlas>)
				.ToArray();
		}
#endif
	}
}
