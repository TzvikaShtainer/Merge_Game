using UnityEngine;
using Zenject;

namespace VisualLayer.GamePlay.Popups.YesNoPopup
{
    public class YesNoPopup : Popup
    {
        #region Factory

        public class Factory : PlaceholderFactory<YesNoPopup> { }

        #endregion
    }
}