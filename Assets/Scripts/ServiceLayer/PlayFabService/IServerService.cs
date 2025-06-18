using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace ServiceLayer.PlayFabService
{
    public interface IServerService
    {
        UniTask<bool>  Login();
        UniTask SetUserData(Dictionary<string, string> data);
        UniTask<Dictionary<string, string>> GetUserData(params string[] keys);
    }
}
