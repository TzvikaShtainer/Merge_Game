namespace ServiceLayer.MusicService
{
    public interface ISfxService
    {
        void PlaySfxType(SfxType type);
        void SetSfxEnabled(bool enabled);
    }
}