using ServiceLayer;

namespace VisualLayer.GamePlay.Popups.DailyRewardPopup
{
    public interface IDailyRewardCooldownService : ICooldownService
    {
        int CurrentDayIndex();
        bool IsWeekComplete();
    }
}