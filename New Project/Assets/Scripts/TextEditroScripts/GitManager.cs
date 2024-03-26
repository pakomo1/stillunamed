using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibGit2Sharp;
using TMPro;
using System.IO;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using System.Text;
public class GitManager : MonoBehaviour
{
    [SerializeField]private GameObject Content;
    [SerializeField]private GameObject changeTemplate;
    [SerializeField]private TextMeshProUGUI totalchangesLbl;
    [SerializeField] private TextMeshProUGUI selectedChangedFile;
    [SerializeField] private InGameTextEditor.TextEditor changesDisplayField;
    private InGameTextEditor.TextEditor textEditor;
    private List<string> filesToCommit = new List<string>();
    private Dictionary<ChangeKind, Sprite> statusImages;

    public event EventHandler onChangesCommitted;

    // Start is called before the first frame update
    void Start()
    {
        LoadStatusImages();
        DisplayChangedFiles();
    }
    private void OnDisable()
    {
        textEditor.disableInput = false;
    }
    private void OnEnable()
    {
        textEditor.disableInput = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DisplayChangedFiles()
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
                changeObject.GetComponent<Button>().onClick.AddListener(()=> { OnChangeClicked(change.Path); });

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
    public async Task<Commit> CommitChangesAsync(string repoPath, string commitSummary,string description, string branchName = "main")
    {
        var user = await GitHubClientProvider.client.User.Current();
        var author = new Signature(user.Login, user.Email ?? $"{user.Login}@users.noreply.github.com", DateTime.Now);
        var committer = author;

        using (var repo = new Repository(repoPath))
        {
            // Check out the desired branch
            Commands.Checkout(repo, branchName);
            foreach (var file in filesToCommit)
            {
                Commands.Stage(repo, file);
            }
            // Commit the changes
            string commitMessage = commitSummary + "\n\n" + description;

            var commit = repo.Commit(commitMessage, author, committer);
            onChangesCommitted?.Invoke(this, EventArgs.Empty);
            return commit;
        }
    }
    private void OnChangeClicked(string filePath)
    {
        using (var repo = new Repository(GameSceneMetadata.githubRepoPath))
        {
            selectedChangedFile.text = filePath;
            // Get the changes between the current version of the file and the version in the last commit
            Patch changes = repo.Diff.Compare<Patch>(repo.Head.Tip.Tree, DiffTargets.WorkingDirectory, new[] { filePath });

            string changesContent = changes.Content;

            // Check if the file is empty
            if (string.IsNullOrEmpty(changesContent))
            {
                // If the file is empty display a custom message
                changesDisplayField.SetText("Empty file");
            }
            else
            {
                // If the file is not empty parse and display the changes
                StringBuilder parsedChanges = new StringBuilder();
                using (StringReader reader = new StringReader(changesContent))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("+") || line.StartsWith("-"))
                        {
                            parsedChanges.AppendLine(line);
                        }
                    }
                }
                print(textEditor.EditorActive);
                changesDisplayField.SetText(parsedChanges.ToString());
            }
        }
    }
    public void Show(InGameTextEditor.TextEditor textEditor)
    {
        this.textEditor = textEditor;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        textEditor.disableInput = false;
    }
}
