using com.ktgame.assets.loader.core;
using com.ktgame.core.manager;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Manager.Assets
{
	public interface IAssetManager : IManager
	{
        SpriteAtlasLoader GetSpriteAtlasLoader(string atlasTag);

		UniTask<TAsset> GetAsset<TAsset>(string key) where TAsset : Object;
        
		AssetRequest<TAsset> Preloaded<TAsset>(string address) where TAsset : Object;

		AssetRequest<TAsset> PreloadedAsync<TAsset>(string address) where TAsset : Object;

		AssetRequest<TAsset> ResourceLoad<TAsset>(string address) where TAsset : Object;

		AssetRequest<Object> AddressableLoad(string address);
		
		AssetRequest<TAsset> ResourceLoadAsync<TAsset>(string address) where TAsset : Object;

		AssetRequest<TAsset> AddressableLoad<TAsset>(string address) where TAsset : Object;

		AssetRequest<TAsset> AddressableLoadAsync<TAsset>(string address) where TAsset : Object;

		void AddressableRelease(AssetRequest request);
	}
}
