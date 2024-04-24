using System;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Vivox;
public class VivoxManager : MonoBehaviour
{
    public static VivoxManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public async void LoginToVivoxAsync(string username)
    {
        LoginOptions options = new LoginOptions();
        options.DisplayName = username;
        options.EnableTTS = true;
        await VivoxService.Instance.LoginAsync(options);
    }
    //joins a channle
    public async void JoinhannelAsync(string channelName)
    {
        Channel3DProperties channel3DProperties = new Channel3DProperties();
        await VivoxService.Instance.JoinPositionalChannelAsync(channelName, ChatCapability.AudioOnly, channel3DProperties);
    }

    public async void LeaveEchoChannelAsync(string channleName)
    {
        string channelToLeave = channleName;
        await VivoxService.Instance.LeaveChannelAsync(channelToLeave);
    } 
    public void LogoutOfVivoxAsync()
    {
        VivoxService.Instance.LogoutAsync();
    }
}
