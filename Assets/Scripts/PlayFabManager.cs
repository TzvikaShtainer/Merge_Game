using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabManager : MonoBehaviour
{
    [SerializeField] private string coins = "100";
    private void Start()
    {
        Login();
        
    }

    private void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    private void OnSuccess(LoginResult obj)
    {
       Debug.Log("OnSuccess");
       //SaveCoins();
       GetCoins();
    }
    
    private void OnError(PlayFabError obj)
    {
        Debug.Log("OnError");
    }

    private void SaveCoins()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "Coins", coins }
            }
        };
        
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnDataSendError);
    }

    private void OnDataSend(UpdateUserDataResult obj)
    {
        Debug.Log("OnDataSend");
    }

    private void OnDataSendError(PlayFabError obj)
    {
        Debug.Log("OnDataSendError");
    }

    private void GetCoins()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnDataReceivedError);
    }

    private void OnDataReceived(GetUserDataResult result)
    {
        Debug.Log("Data Received");

        if (result.Data != null
            && result.Data.ContainsKey("Coins"))
        {
            coins = result.Data["Coins"].Value;
        }
        else
        {
            Debug.Log("Player Data Not Complete!");
        }
    }

    private void OnDataReceivedError(PlayFabError obj)
    {
        Debug.Log("OnDataReceivedError");
    }
}
