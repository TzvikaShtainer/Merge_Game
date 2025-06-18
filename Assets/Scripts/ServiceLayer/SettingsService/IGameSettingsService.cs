using Cysharp.Threading.Tasks;

namespace ServiceLayer.SettingsService
{
    public interface IGameSettingsService
    {
        GameSettings Settings { get; }
        void SetMusic(bool isOn);
        void SetSoundEffects(bool isOn);
        void SetVibration(bool isOn);

        UniTask LoadFromServer();
    }
}