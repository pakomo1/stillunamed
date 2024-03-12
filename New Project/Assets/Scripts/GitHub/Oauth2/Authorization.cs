using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;

public class Authorization : MonoBehaviour
{
    [SerializeField] private Oauth2AccessToken Oauth2AC;
    [SerializeField] private DeviceFlowToken deviceFlowToken;
    [SerializeField] private GameObject loadingMessage;
    [SerializeField] private GameObject authorizationButton;
    string clientId = "e4edd56045a448fbdc0f";
    string systemRedirectUri;

    void Start()
    {
        systemRedirectUri = OAuth2.CreateRedirect(
            new OAuth2Provider
            {
            ClientId = clientId,
            Scope = "repo",
            AuthUri = "https://github.com/login/oauth/authorize",
            AccessTokenUri = "https://github.com/login/oauth/access_token"
            },
            "https://eog0ubb2uup5m2r.m.pipedream.net");
    }

    
    public void Open()
    {
        authorizationButton.SetActive(false);
        loadingMessage.SetActive(true);
        deviceFlowToken.GetDeviceCode();
    }
}
