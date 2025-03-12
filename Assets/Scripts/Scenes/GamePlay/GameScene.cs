using System.Linq;
using com.ktgame.core;
using com.ktgame.core.di;
using com.ktgame.core.manager;
using com.ktgame.manager.ui;
using com.ktgame.services.scene;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace Scenes.GamePlay
{
    public class GameScene: Scene
    {
        [ShowInInspector, ReadOnly] private IManager[] _managers;

        [Inject] private IGame _game;
        [Inject] private readonly IInjector _injector;
        [Inject] private readonly ISceneService _sceneService;
        
        private IUIManager _uiManager;
        
        protected override void Awake()
        {
            base.Awake();
            _managers = GetComponentsInChildren<IManager>();
        }

        protected override UniTask OnInstallManagers(IManagerInstaller installer)
        {
            foreach (var manager in _managers)
            {
                var managerType = manager.GetType();
                
                var managerInterface = managerType
                    .GetInterfaces()
                    .FirstOrDefault(i => typeof(IManager).IsAssignableFrom(i) && i != typeof(IManager));

                installer.Binding(managerInterface, manager);
            }
            
            return UniTask.CompletedTask;
        }

        protected override UniTask OnEnter()
        {
            //_game = (IGame)Game.Instance;
            _uiManager = GetManager<IUIManager>();
            
            return UniTask.CompletedTask;
        }
        
        
        protected override UniTask OnExit()
        {
            return UniTask.CompletedTask;
        }
    }
}