using System;
using System.Collections.Generic;
using com.ktgame.assets.loader.core;
using UnityEngine;

namespace Manager.Assets
{
	[Serializable]
	public class SpriteAtlasSet
	{
		[SerializeField] private string _groupName;
		[SerializeField] private SpriteAtlasLoader[] _atlasLoaders;

		private Dictionary<string, SpriteAtlasLoader> _atlasLookup;

		public string GroupName => _groupName;

		public SpriteAtlasLoader[] AtlasLoaders
		{
			set => _atlasLoaders = value;
			get => _atlasLoaders;
		}

		public void InitLookup(IAssetLoader assetLoader)
		{
			_atlasLookup = new Dictionary<string, SpriteAtlasLoader>(_atlasLoaders.Length);
			foreach (var atlasLoader in _atlasLoaders)
			{
				atlasLoader.AssetLoader = assetLoader;
				_atlasLookup.Add(atlasLoader.AtlasTag, atlasLoader);
			}
		}

		public SpriteAtlasLoader GetSpriteLoader(string atlasTag)
		{
			return _atlasLookup[atlasTag];
		}

		public bool HasAtlasLoader(string atlasTag)
		{
			return _atlasLookup.ContainsKey(atlasTag);
		}

		public void ReleaseAllAtlases()
		{
			foreach (var atlasLoader in _atlasLoaders)
			{
				atlasLoader.UnloadAtlas();
			}
		}

		public void ReleaseUnusedAtlases()
		{
			foreach (var atlasLoader in _atlasLoaders)
			{
				if (atlasLoader.Releasable)
				{
					atlasLoader.UnloadAtlas();
				}
			}
		}

		public void ResetRefCount()
		{
			foreach (var atlasLoader in _atlasLoaders)
			{
				atlasLoader.ResetRefCount();
			}
		}
	}
}
