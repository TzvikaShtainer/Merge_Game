using System;
using Cysharp.Threading.Tasks;

namespace ServiceLayer.HourlyCoinsService
{
    public interface IHourlyCoinsService
    {
        UniTask LoadFromServer();
        bool CanClaim(out TimeSpan timeRemaining);
        void Claim();
        DateTime LastClaimTimeUtc { get; }
        int GetHourlyCoinsAmount();
    }
}