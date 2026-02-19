using System;
using Cysharp.Threading.Tasks;

namespace ServiceLayer.TImeProvider
{
    public interface ITimeProviderService
    {
        UniTask<DateTime> GetServerTimeUtc();
    }
}