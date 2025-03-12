using com.ktgame.core.di;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace com.ktgame.core
{
	public class Game : Architecture<Game>, IGame
	{
		public bool IsSupporter { get; set; }
		
		public override bool InjectSceneLoadedDependencies => false;

		[Inject] private readonly ILogger _logger;

		private bool _isDevelopmentEnvironment;

		protected override UniTask OnPreInitialize()
		{
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			Application.targetFrameRate = 60;
			System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

#if DEVELOPMENT
			_isDevelopmentEnvironment = true;
#endif
			return base.OnPreInitialize();
		}

		protected override UniTask OnPostInitialize()
		{
			Injector.Resolve(this);
			Injector.AddSingleton(this, typeof(IGame));
			return base.OnPostInitialize();
		}

		private void OnConfigFetchSuccessHandler()
		{
			
		}
	}
}
