using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Octokit;
using UnityEngine.Networking;
using System.Net.Http;
using UnityEngine.TextCore.Text;
public class CommitButtonTemplate : MonoBehaviour
{
    [SerializeField]private GameObject commitButtonTemplate;
    public async void CreateButtonsForCommits(IReadOnlyList<GitHubCommit> commits)
    {
        ClearCommits();
        foreach (var commit in commits)
        {
            var button = Instantiate(commitButtonTemplate, transform);
            button.SetActive(true);

            TextMeshProUGUI commitMessageTextMesh = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            commitMessageTextMesh.overflowMode = TextOverflowModes.Ellipsis;
            commitMessageTextMesh.text = commit.Commit.Message;

            var footer = button.transform.GetChild(1);

            // Set the avatar image
           Sprite avatarImage = await GetProfilePicture(commit.Author.AvatarUrl);
            footer.GetChild(0).GetChild(0).GetComponent<Image>().sprite = avatarImage;

            TextMeshProUGUI authorTextMesh = footer.GetChild(1).GetComponent<TextMeshProUGUI>();
            authorTextMesh.overflowMode = TextOverflowModes.Ellipsis;
            authorTextMesh.text = commit.Commit.Author.Name;

            TextMeshProUGUI dateTextMesh = footer.GetChild(2).GetComponent<TextMeshProUGUI>();
            dateTextMesh.overflowMode = TextOverflowModes.Ellipsis;
            dateTextMesh.text = GetRelativeTime(commit.Commit.Author.Date);
        }

    }

    private async Task<Sprite> GetProfilePicture(string avatar)
    {
        using (var httpClient = new HttpClient())
        {
            var imageData =  await httpClient.GetByteArrayAsync(avatar);

            var texture = new Texture2D(2, 2);
            texture.LoadImage(imageData); 
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
    }
    //clears the commits 
    public void ClearCommits()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
    private string GetRelativeTime(DateTimeOffset dateTime)
    {
        var timeSpan = DateTimeOffset.Now - dateTime;
        if (timeSpan.TotalDays > 365)
        {
            int years = (int)(timeSpan.TotalDays / 365);
            return $"{years} year{(years > 1 ? "s" : "")} ago";
        }
        if (timeSpan.TotalDays > 30)
        {
            int months = (int)(timeSpan.TotalDays / 30);
            return $"{months} month{(months > 1 ? "s" : "")} ago";
        }
        if (timeSpan.TotalDays >= 1)
        {
            int days = (int)timeSpan.TotalDays;
            return $"{days} day{(days > 1 ? "s" : "")} ago";
        }
        if (timeSpan.TotalHours >= 1)
        {
            int hours = (int)timeSpan.TotalHours;
            return $"{hours} hour{(hours > 1 ? "s" : "")} ago";
        }
        if (timeSpan.TotalMinutes >= 1)
        {
            int minutes = (int)timeSpan.TotalMinutes;
            return $"{minutes} minute{(minutes > 1 ? "s" : "")} ago";
        }
        int seconds = (int)timeSpan.TotalSeconds;
        return $"{seconds} second{(seconds > 1 ? "s" : "")} ago";
    }
}
