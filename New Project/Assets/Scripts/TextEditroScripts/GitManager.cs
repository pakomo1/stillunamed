using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibGit2Sharp;
using TMPro;
using System.IO;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
public class GitManager : MonoBehaviour
{
    [SerializeField]private GameObject Content;
    [SerializeField]private GameObject changeTemplate;
    [SerializeField]private TextMeshProUGUI totalchangesLbl;
    private List<string> filesToCommit = new List<string>();
    private Dictionary<ChangeKind, Sprite> statusImages;

    // Start is called before the first frame update
    void Start()
    {
        LoadStatusImages();
        DisplayChangedFiles();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void DisplayChangedFiles()
    {
        using (var repo = new Repository(GameSceneMetadata.githubRepoPath))
        {
            var changes = repo.Diff.Compare<TreeChanges>(repo.Head.Tip.Tree, DiffTargets.Index | DiffTargets.WorkingDirectory);
            totalchangesLbl.text = "Total Changes: " + changes.Count;
            foreach (var change in changes)
            {
                Debug.Log(change.Path);
                GameObject changeObject = Instantiate(changeTemplate);
                changeObject.transform.SetParent(Content.transform);

                string directory = Path.GetDirectoryName(change.Path);
                string fileName = Path.GetFileName(change.Path);

                 filesToCommit.Add(change.Path);
                changeObject.transform.GetChild(0).GetComponent<Toggle>().onValueChanged.AddListener((value) => OnToggleChanged(value, change.Path));

                changeObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"<color=#797979>{directory}/</color>{fileName}";
                changeObject.GetComponent<Button>().onClick.AddListener(OnChangeClicked);

                var statusImage = changeObject.transform.GetChild(2).GetComponent<Image>();
                statusImage.sprite = statusImages[change.Status];

                changeObject.SetActive(true);
            }
        }
    }

    private void OnToggleChanged(bool value, string path)
    {
        if (value)
        {
            // If the toggle is checked, add the file to the list
            if (!filesToCommit.Contains(path))
            {
                print("Added " + path); 
                filesToCommit.Add(path);
            }
        }
        else
        {
            // If the toggle is unchecked, remove the file from the list
            filesToCommit.Remove(path);
        }
    }

    public async Task<Commit> CommitChangesAsync(string repoPath, string commitMessage)
    {
        var user = await GitHubClientProvider.client.User.Current();
        var author = new Signature(user.Name, user.Email, DateTime.Now);
        var committer = author;

        using (var repo = new Repository(repoPath))
        {
            foreach (var file in filesToCommit)
            {
                Commands.Stage(repo, file);
            }
            // Commit the changes
            var commit = repo.Commit(commitMessage, author, committer);
            return commit;
        }
    }
    private void LoadStatusImages()
    {
        statusImages = new Dictionary<ChangeKind, Sprite>();
        foreach (ChangeKind status in Enum.GetValues(typeof(ChangeKind)))
        {
            string statusName = Enum.GetName(typeof(ChangeKind), status);
            Sprite statusImage = Resources.Load<Sprite>($"Status/{statusName}");
            statusImages[status] = statusImage;
        }
    }
    private void OnChangeClicked()
    {
        Debug.Log("Change Clicked");
    }   
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
