namespace ServiceLayer.MusicService
{
    public interface  IMusicService
    {
        void PlayLoadingMusic();
        void PlayGameMusic();
        void StopMusic();
        void SetMusicEnabled(bool enabled);
        bool IsMusicEnabled { get; }
    }
}