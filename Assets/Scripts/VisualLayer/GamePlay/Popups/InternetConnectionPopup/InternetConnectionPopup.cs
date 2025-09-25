using VisualLayer.GamePlay.Popups.YesNoPopup;
using Zenject;

namespace VisualLayer.GamePlay.Popups.InternetConnectionPopup
{
    public class InternetConnectionPopup : YesNoPopup.YesNoPopup
    {
        public new class Factory : PlaceholderFactory<YesNoPopupArgs, InternetConnectionPopup> { }

    }
}