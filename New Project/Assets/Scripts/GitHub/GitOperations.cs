using LibGit2Sharp;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class GitOperations : MonoBehaviour
{  
    public static Action OnConflictsFound;
    //check if the user is the owner of the repository
    public static async Task<bool> IsUserRepoOwnerAsync(string username, string repoLink)
    {
        var url = new Uri(repoLink);
        var repoName = url.Segments.Last();
        var ownerName = url.Segments[url.Segments.Length - 2].Trim('/');

        var repo = await GitHubClientProvider.client.Repository.Get(ownerName, repoName);
        return repo.Owner.Login == username;
    }
    // Check if the user is a collaborator of the repository
    public static async Task<bool> IsUserCollaboratorAsync(string username, string repoLink)
    {
        var url = new Uri(repoLink);
        var repoName = url.Segments.Last();
        var ownerName = url.Segments[url.Segments.Length - 2].Trim('/');

        var collaborators = await GitHubClientProvider.client.Repository.Collaborator.GetAll(ownerName, repoName);
        return collaborators.Any(c => c.Login == username);
    }
    //invites a user to a repository
    public static async Task InviteUserToRepoAsync(string username, string repoLink)
    {
        var url = new Uri(repoLink);
        var repoName = url.Segments.Last();
        var ownerName = url.Segments[url.Segments.Length - 2].Trim('/');

        await GitHubClientProvider.client.Repository.Collaborator.Invite(ownerName, repoName, username);
    }

    public static int CountUnpushedCommits(string repoPath, string branchName = "main")
    {
        using (var repo = new LibGit2Sharp.Repository(repoPath))
        {
            var localBranch = repo.Branches[branchName];
            var trackingBranch = localBranch.TrackedBranch;
            if (trackingBranch == null)
            {
                // If there is no tracking branch then all commits are unpushed
                return localBranch.Commits.Count();
            }
            else
            {
                // If there is a tracking branch then count the commits that are in the local branch but not in the tracking branch
                return localBranch.Commits.TakeWhile(c => c != trackingBranch.Tip).Count();
            }
        }
    }
    public static async Task<string> CloneRepositoryAsync(string sourceUrl, string destinationPath)
    {
        var co = new CloneOptions();
        var credentails = await GetCredentialsAsync();
        co.CredentialsProvider = (_url, _user, _cred) => credentails;

        Directory.CreateDirectory(destinationPath);
        string clonedRepoPath = await Task.Run(() => LibGit2Sharp.Repository.Clone(sourceUrl, destinationPath, co));
        return clonedRepoPath;
    }

    public static async Task PushChangesAsync(string repoPath, string branchName = "main")
    {
        using (var repo = new LibGit2Sharp.Repository(repoPath))
        {
            try
            {
                var credentials = await GetCredentialsAsync();
                var localBranch = repo.Branches[branchName];
                var remote = repo.Network.Remotes["origin"];

                // Pull the latest changes
                await PullChangesAsync(repoPath, branchName);

                // Check for conflicts
                if (repo.Index.Conflicts.Any())
                {
                    // If there are unresolved conflicts, get the conflicted files
                    List<string> conflictedFiles = GetConflictedFiles(repoPath);

                    OnConflictsFound?.Invoke();
                    // Throw an exception to stop the push
                    throw new Exception("There are unresolved conflicts. Please resolve them before pushing changes.");
                }

                var pushOptions = new PushOptions
                {
                    CredentialsProvider = (_url, _user, _cred) => credentials
                };

                repo.Network.Push(remote, @"refs/heads/" + branchName, pushOptions);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public static async Task PullChangesAsync(string repoPath, string branchName = "main")
    {
        using (var repo = new LibGit2Sharp.Repository(repoPath))
        {
            try
            {
                var credentials = await GetCredentialsAsync();
                var pullOptions = new PullOptions
                {
                    FetchOptions = new FetchOptions
                    {
                        CredentialsProvider = (_url, _user, _cred) => credentials
                    }
                };
                var user = await GitHubClientProvider.client.User.Current();
                var email = user.Email ?? $"{user.Login}@users.noreply.github.com";
                // Pull the latest changes
                Commands.Pull(repo, new LibGit2Sharp.Signature(credentials.Username, email, DateTimeOffset.Now), pullOptions);
            }catch(Exception ex)
            {
                throw ex;
            }
          
        }
    }
    public static async Task<UsernamePasswordCredentials> GetCredentialsAsync()
    {
        var user = await GitHubClientProvider.client.User.Current();
        string username = user.Login;
        var credentials = await FileCredentialStore.Instance.GetAccessToken();
        string token = credentials.Password;

        return new UsernamePasswordCredentials { Username = username, Password = token };
    }
    public static List<string> GetConflictedFiles(string repoPath)
    {
        using (var repo = new LibGit2Sharp.Repository(repoPath))
        {
            var conflicts = repo.Index.Conflicts;
            List<string> conflictedFiles = new List<string>();

            foreach (var conflict in conflicts)
            {
                conflictedFiles.Add(conflict.Ancestor.Path);
            }

            return conflictedFiles;
        }
    }
    public static bool HasUnresolvedConflicts(string repoPath)
    {
        using (var repo = new LibGit2Sharp.Repository(repoPath))
        {
            return repo.Index.Conflicts.Any();
        }
    }
    public static void MarkFileAsResolved(string repoPath, string filePath)
    {
        using (var repo = new LibGit2Sharp.Repository(repoPath))
        {
            Commands.Stage(repo, filePath);
        }
    }
    public static bool HasChanges(string repoPath)
    {
        using (var repo = new LibGit2Sharp.Repository(repoPath))
        {
            var changes = repo.Diff.Compare<TreeChanges>(repo.Head.Tip.Tree, DiffTargets.Index | DiffTargets.WorkingDirectory);
            return changes.Count() > 0;
        }
    }
    public static void SwitchBranch(string repositoryPath, string branchName)
    {
        using (var repo = new LibGit2Sharp.Repository(repositoryPath))
        {
            var branch = repo.Branches[branchName];

            // If the branch doesn't exist locally, create a tracking branch
            if (branch == null)
            {
                var remoteBranch = repo.Branches[$"origin/{branchName}"];
                if (remoteBranch == null)
                {
                    throw new Exception($"Branch {branchName} does not exist");
                }

                branch = repo.CreateBranch(branchName, remoteBranch.Tip);
                repo.Branches.Update(branch, b => b.TrackedBranch = remoteBranch.CanonicalName);
            }

            Commands.Checkout(repo, branch);
        }
    }
}
