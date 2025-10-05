using System;
using Cysharp.Threading.Tasks;
using ServiceLayer.PlayFabService;

namespace ServiceLayer.HourlyCoinsService
{
    public interface IHourlyCoinsService
    {
        UniTask LoadFromServer();
        UniTask<DateTime> GetServerTimeUtc();
        bool CanClaim(out TimeSpan timeRemaining);
        void Claim();
        DateTime LastClaimTimeUtc { get; }
        int GetHourlyCoinsAmount();
    }
}