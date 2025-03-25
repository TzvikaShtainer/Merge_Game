namespace VisualLayer.GamePlay.Popups.YesNoPopup
{
    public class YesNoPopupResult
    {
        public bool IsYes;

        public bool IsNo => !IsYes;
    }
}