using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;

public class Authorization : MonoBehaviour
{
    [SerializeField] private Oauth2AccessToken Oauth2AC;
    [SerializeField] private GameObject loadingMessage;
    [SerializeField] private GameObject authorizationButton;
    string clientId = "086490aaf0f9ef0b33e4";
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
            "http://localhost:3000/callback/");
    }

    public void Open()
    {
        authorizationButton.SetActive(false);
        loadingMessage.SetActive(true);
        Application.OpenURL(systemRedirectUri);
        Oauth2AC.AuthorizeBegin();
    }
}
