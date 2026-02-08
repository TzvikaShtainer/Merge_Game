using System;
using System.Collections.Generic;

namespace ServiceLayer.DataSyncService
{
    public interface ISyncableService
    {
        event Action OnDataChanged;
        Dictionary<string, string> GetSyncData();
    }
}