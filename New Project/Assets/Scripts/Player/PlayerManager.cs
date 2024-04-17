using System.Collections;
using UnityEngine;
using Unity.Netcode;
using System;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Authentication;
using System.Linq;
using Unity.Services.Lobbies.Models;
using System.Threading;
public class PlayerManager : NetworkBehaviour
{
    private string _username;
    private TextEditorData _textEditor;
    private TaskCompletionSource<bool> _usernameSetTcs = new TaskCompletionSource<bool>();

    public static event EventHandler<PlayerSpawnArgs> OnAnyPlayerSpawn;
    //[SerializeField] private TextMeshProUGUI _usernameText;
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

       _= SetUsernameAsync();

        StopInteractingWithUI();
        LocalPlayer = this;

        if (IsHost)
        {
            ComputersManager.ComputersInitialized.OnValueChanged += ComputersInitialized_OnValueChanged;
        }
        else if (IsClient)
        {
            StartCoroutine(WaitForComputersInitialization());
        }
    }


    private async void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        try
        {
            string playerId = AuthenticationService.Instance.PlayerId;
            await LobbyService.Instance.RemovePlayerAsync("lobbyId", playerId);
        }catch(Exception e)
        {
            Debug.LogError(e.Message);
        }   
    }
    private async Task SetUsernameAsync()
    {
        try
        {
            string currentUsername = await GetGitUsernme();
            SetUsername(currentUsername);
            _usernameSetTcs.SetResult(true);
            print("Username set " + OwnerClientId);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    //executes when the computers are initialized
    private void ComputersInitialized_OnValueChanged(bool previousValue, bool newValue)
    {
        _ = SetEditorAsync();
    }
    //executes when the client is connected
    private IEnumerator WaitForComputersInitialization()
    {
        yield return new WaitUntil(() => ComputersManager.ComputersInitialized.Value);
        _ = SetEditorAsync();
    }
    private async Task<string> GetGitUsernme()
    {
        var user = await GitHubClientProvider.client.User.Current();
        return user.Login;
    }

    private async Task SetEditorAsync()
    {
        await _usernameSetTcs.Task;
        var room = GameObject.Find("Room");
        var computers = room.transform.Find("Computers");

        for (int i = 0; i < computers.childCount; i++)
        {
            var computer = computers.GetChild(i);
            
            if (computer.childCount == 0)
            {
                RequestEditorSpawnServerRpc(i, _username, GameSceneMetadata.GithubRepoPath);
                break;
            }

        }
    }

    [ServerRpc]
    private void RequestEditorSpawnServerRpc(int computerId,string username, string dir)
    {
        var room = GameObject.Find("Room");
        var computers = room.transform.Find("Computers");
        var computer = computers.GetChild(computerId);

        // Instantiate the text editor for this computer and make it a child of the computer
        GameObject Editor = Instantiate(EditorDataPrefab);
        NetworkObject networkObjectEditor = Editor.GetComponent<NetworkObject>();
      
        networkObjectEditor.Spawn();

        Editor.transform.SetParent(computer.transform);
        var thiseditorData = Editor.GetComponent<TextEditorData>();
        thiseditorData.Id = computerId;
        Editor.SetActive(false);

        //assign it to the player
        SetTextEditor(Editor.GetComponent<TextEditorData>());
        thiseditorData.SetOwner(username);
        networkObjectEditor.ChangeOwnership(OwnerClientId);
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
}
public class PlayerSpawnArgs : EventArgs
{
    public string Username { get; set; }
}
