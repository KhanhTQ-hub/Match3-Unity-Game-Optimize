using System;
using com.ktgame.assets.loader.core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.U2D;

namespace Manager.Assets
{
	[Serializable]
	public class SpriteAtlasLoader
	{
		[SerializeField, ReadOnly] private string _tag;
		[SerializeField, ReadOnly] private string _atlasPath;

		public string AtlasTag => _tag;

		public int RefCount { private set; get; }

		public bool IsLoaded => _atlas != null;

		public bool Releasable => RefCount == 0;

		public IAssetLoader AssetLoader { set; private get; }

		public AssetRequest<SpriteAtlas> AssetRequest { get; private set; }

		private SpriteAtlas _atlas;

		public SpriteAtlasLoader(string atlasTag, string atlasPath)
		{
			_tag = atlasTag;
			_atlasPath = atlasPath;
		}

		public SpriteAtlas Atlas
		{
			get
			{
				if (_atlas == null)
				{
					AssetRequest = AssetLoader.Load<SpriteAtlas>(_atlasPath);
					_atlas = AssetRequest.Result;
				}

				return _atlas;
			}
		}

		public void IncreaseRefCount()
		{
			RefCount++;
		}

		public void ResetRefCount()
		{
			RefCount = 0;
		}

		public void UnloadAtlas()
		{
			if (!IsLoaded)
			{
				return;
			}

			if (AssetRequest != null)
			{
				AssetLoader.Release(AssetRequest);
			}

			_atlas = null;
			AssetRequest = null;
			RefCount = 0;
		}
	}
}
