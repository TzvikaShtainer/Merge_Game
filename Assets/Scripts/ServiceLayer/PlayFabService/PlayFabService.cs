using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace ServiceLayer.PlayFabService
{
    public class PlayFabService : IServerService
    {
        public async UniTask<bool> Login()
        {
            var tcs = new UniTaskCompletionSource<bool>();
            
            var request = new LoginWithCustomIDRequest
            {
                CustomId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true
            };
            
            PlayFabClientAPI.LoginWithCustomID(request,
                result =>
                {
                    Debug.Log("Login Success");
                    tcs.TrySetResult(true);
                },
                error =>
                {
                    Debug.Log("Login Failed");
                    tcs.TrySetResult(false);
                });
            
            return await tcs.Task;
        }

        public async UniTask SetUserData(Dictionary<string, string> data)
        {
            var tcs = new UniTaskCompletionSource<UpdateUserDataResult>();

            var request = new UpdateUserDataRequest
            {
                Data = data
            };
            
            PlayFabClientAPI.UpdateUserData(request,
                result => tcs.TrySetResult(result),
                error =>
                {
                    Debug.Log("UpdateUserData Failed");
                    tcs.TrySetException(new System.Exception(error.ErrorMessage));
                });
            
            await tcs.Task;
        }

        public async UniTask<Dictionary<string, string>> GetUserData(params string[] keys)
        {
            var tcs = new UniTaskCompletionSource<GetUserDataResult>();

            var request = new GetUserDataRequest();
            if (keys?.Length > 0)
            {
                request.Keys = keys.ToList();
            }
            
            PlayFabClientAPI.GetUserData(request,
                result => tcs.TrySetResult(result),
                error =>
                {
                    Debug.LogError($"❌ GetUserData failed: {error.ErrorMessage}");
                    tcs.TrySetException(new System.Exception(error.ErrorMessage));
                });
            
            var response = await tcs.Task;
            
            return response.Data?.ToDictionary(pair => pair.Key, pair => pair.Value.Value)
                   ?? new Dictionary<string, string>();
        }
    }
}