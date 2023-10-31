using Octokit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GitHubClientProvider
{

    public static GitHubClient client = new GitHubClient(new ProductHeaderValue("StillunamedApp"));
    public static GitHubClient GetGitHubClient(string accessToken)
    {
        client.Credentials = new Credentials(accessToken);
        return client;
    }
}
