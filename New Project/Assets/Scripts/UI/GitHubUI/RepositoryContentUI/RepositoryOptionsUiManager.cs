using Octokit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepositoryOptionsUiManager : MonoBehaviour
{
    private Repository selectedRepo;
    [SerializeField] private LobbyUi lobbyUi;
    [SerializeField] private CreateLobbyUI createLobbyUI;
    [SerializeField] private GameObject githubMainPanel;
    [SerializeField] private Button createLobbyButton;
   public void Show(Repository repository)   
   {
        selectedRepo = repository;
        gameObject.SetActive(true);
   }
   public void Hide()
   {
    
   }
    private void Start()
    {
        createLobbyButton.onClick.AddListener(createLobbyOnClickHandler);
    }
    void Update()
    {
       /* if (Input.GetMouseButtonDown(0))
        {
           gameObject.SetActive(false);
        }*/
    }

    public void createLobbyOnClickHandler()
    {
        print("The button has been pressed");
        githubMainPanel.SetActive(false);

        lobbyUi.Show();
        createLobbyUI.Show(selectedRepo);
    }
}
