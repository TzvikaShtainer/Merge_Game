using System;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;

namespace ServiceLayer.TImeProvider
{
    public class TimeProviderService : ITimeProviderService
    {
        public async UniTask<DateTime> GetServerTimeUtc()
        {
            var tcs = new UniTaskCompletionSource<DateTime>();
            PlayFabClientAPI.GetTime(new GetTimeRequest(),
                results => tcs.TrySetResult(results.Time.ToUniversalTime()),
                error => tcs.TrySetResult(DateTime.UtcNow));
        
            return await tcs.Task;
        }
    }
}