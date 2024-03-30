using System.Collections;
using UnityEngine;
using Unity.Netcode;
using System;
using System.Threading.Tasks;
public class PlayerManager : NetworkBehaviour
{
    private string _username;
    private TextEditorData _textEditor;
    public static event EventHandler<PlayerSpawnArgs> OnAnyPlayerSpawn;
  //  [SerializeField] private TextMeshProUGUI _usernameText;
    [SerializeField] private GameObject EditorDataPrefab;
    public static event EventHandler OnEditorSpawned; 

    public static PlayerManager LocalPlayer { get; private set; }
    public string GetUsername()
    {
        return _username;
    }
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { return; }

        _ = Task.Run(async () =>
        {
            try
            {
                string currentUsername = await GetGitUsernme();
                SetUsername(currentUsername);

                var isOwner = await GitOperations.IsUserRepoOwnerAsync(_username, GameSceneMetadata.GithubRepoLink);
                if (isOwner)
                {
                    GameLobby.OnPlayerTriesToJoin += GameLobby_OnPlayerTriesToJoin;
                }
            }catch(Exception e)
            {
                Debug.LogError(e.Message);
            }
        });
       // _username = "pakomo1";
        StopInteractingWithUI();
        LocalPlayer = this;

        if (IsHost)
        {
            ComputersManager.ComputersInitialized.OnValueChanged += ComputersInitialized_OnValueChanged;
        }else if (IsClient)
        {
            StartCoroutine(WaitForComputersInitialization());
        }
    }

    private async void GameLobby_OnPlayerTriesToJoin(object sender, LobbyJoinEventArgs e)
    {
        string username = e.Username;
        bool isCollaborator = await GitOperations.IsUserCollaboratorAsync(username, GameSceneMetadata.GithubRepoLink);
        if (!isCollaborator)
        {
            await GitOperations.InviteUserToRepoAsync(username, GameSceneMetadata.GithubRepoLink);
        }
    }

    public void StartInteractingWithUI()
    {
        GetComponent<PlayerMovement>().IsInteractingWithUI = true;
    }
    public void StopInteractingWithUI()
    {
        GetComponent<PlayerMovement>().IsInteractingWithUI = false;
    }
    public bool isPlayerInteracting()
    {
        return GetComponent<PlayerMovement>().IsInteractingWithUI;
    }

    private void ComputersInitialized_OnValueChanged(bool previousValue, bool newValue)
    {
        GetEditor();
    }
    private IEnumerator WaitForComputersInitialization()
    {
        yield return new WaitUntil(() => ComputersManager.ComputersInitialized.Value);
        GetEditor();
    }
    private async Task<string> GetGitUsernme()
    {
        var user = await GitHubClientProvider.client.User.Current();
        return user.Login;
    }
   
    private void GetEditor()
    {
        var room = GameObject.Find("Room");
        var computers = room.transform.Find("Computers");

        for (int i = 0; i < computers.childCount; i++)
        {
            var computer = computers.GetChild(i);
            
            if (computer.childCount == 0)
            {
                RequestEditorSpawnServerRpc(i, _username);
                break;
            }

        }
    }

    [ServerRpc]
    private void RequestEditorSpawnServerRpc(int computerId,string username)
    {
        var room = GameObject.Find("Room");
        var computers = room.transform.Find("Computers");
        var computer = computers.GetChild(computerId);

        // Instantiate the text editor for this computer and make it a child of the computer
        GameObject Editor = Instantiate(EditorDataPrefab);
        NetworkObject networkObjectEditor = Editor.GetComponent<NetworkObject>();
        if (networkObjectEditor != null)
        {
            networkObjectEditor.Spawn();
            networkObjectEditor.ChangeOwnership(OwnerClientId);
        }
        Editor.transform.SetParent(computer.transform);
        var thiseditorData = Editor.GetComponent<TextEditorData>();
        thiseditorData.Id = computerId;
        thiseditorData.InitializePaths();
        Editor.SetActive(false);

        //assign it to the player
        SetTextEditor(Editor.GetComponent<TextEditorData>());
        thiseditorData.SetOwner(username);
        OnEditorSpawned?.Invoke(this, EventArgs.Empty);
    }
    public void SetUsername(string username)
    {
        _username = username;
    }
    public TextEditorData GetTextEditor()
    {
        return _textEditor;
    }

    public void SetTextEditor(TextEditorData textEditor)
    {
        _textEditor = textEditor;
    }
   
}
public class PlayerSpawnArgs : EventArgs
{
    public string Username { get; set; }
}
