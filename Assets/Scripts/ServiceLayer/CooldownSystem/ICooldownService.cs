using System;
using Cysharp.Threading.Tasks;

namespace ServiceLayer
{
    public interface ICooldownService
    {
        UniTask LoadFromServer();
        bool CanClaim(out TimeSpan timeRemaining);
        void Claim();
        DateTime LastClaimTimeUtc { get; }
        int GetRewardAmount();
    }
}