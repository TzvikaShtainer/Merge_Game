using FMODUnity;

namespace ServiceLayer.MusicService
{
    public interface  IMusicService
    {
        void PlayMusic(EventReference musicRef);
        void PlayLoadingMusic();
        void PlayGameMusic();
        void StopMusic();
        void SetMusicEnabled(bool enabled);
        bool IsMusicEnabled { get; }
    }
}