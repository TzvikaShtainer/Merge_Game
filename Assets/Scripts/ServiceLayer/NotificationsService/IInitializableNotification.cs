using Cysharp.Threading.Tasks;

namespace ServiceLayer.NotificationsService
{
    public interface IInitializableNotification
    {
        int Priority { get; }
        UniTask<bool> ShouldShow();
        UniTask Show();
        
    }
}