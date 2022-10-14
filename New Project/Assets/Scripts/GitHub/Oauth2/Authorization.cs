using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Authorization : MonoBehaviour
{
    [SerializeField] private Oauth2AccessToken Oauth2AC;
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
        Application.OpenURL(systemRedirectUri);
        Oauth2AC.AuthorizeBegin();
    }
}
