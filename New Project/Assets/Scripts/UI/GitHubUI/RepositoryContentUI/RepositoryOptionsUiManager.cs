using Octokit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositoryOptionsUiManager : MonoBehaviour
{
    private Repository selectedRepo;
    [SerializeField] private LobbyUi lobbyUi;
    [SerializeField] private CreateLobbyUI createLobbyUI;
    [SerializeField] private GameObject githubMainPanel;

   public void Show(Repository repository)   
   {
        selectedRepo = repository;
        gameObject.SetActive(true);
   }
   public void Hide()
   {
    
   }

    public void createLobbyOnClickHandler()
    {
        githubMainPanel.SetActive(false);

        lobbyUi.Show();
        createLobbyUI.Show(selectedRepo);
    }
}
