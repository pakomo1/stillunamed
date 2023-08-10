using Octokit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GitHubClientProvider
{

    public static GitHubClient client;
    public static GitHubClient GetGitHubClient(string accessToken)
    {
        client = new GitHubClient(new ProductHeaderValue("TestApp"));
        client.Credentials = new Credentials(accessToken);
        return client;
    }
}
