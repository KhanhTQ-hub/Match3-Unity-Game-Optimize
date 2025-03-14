using com.ktgame.manager.ui;

namespace View
{
    public class PanelMainPresenter : ModalPresenter<PanelMainView>
    {
        public PanelMainPresenter(IUIManager uiManager, IViewContainer viewContainer, IViewConfig viewConfig) : base(uiManager, viewContainer, viewConfig)
        {
        }

        protected override void AddChildren()
        {
            
        }
    }
}
