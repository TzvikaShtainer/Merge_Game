namespace ServiceLayer.DataSyncService
{
    public interface ISyncLock
    {
        void LockSync();
        void UnlockSync(); 
    }
}