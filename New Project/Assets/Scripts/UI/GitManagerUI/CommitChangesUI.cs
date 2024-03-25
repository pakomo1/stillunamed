using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using Unity.VisualScripting;
public class CommitChangesUI : MonoBehaviour
{
    [SerializeField]private TMP_InputField commitMessageInput;
    [SerializeField]private TMP_InputField commitDescription;
   // [SerializeField]private TextMeshProUGUI changesCount;
    [SerializeField]private Button commitButton;
    [SerializeField]private GitManager gitManager;
    // Start is called before the first frame update
    void Start()
    {
        commitButton.onClick.AddListener(CommitChangesAsync);
    }

    private async void CommitChangesAsync()
    {
        try
        {
            await gitManager.CommitChangesAsync(GameSceneMetadata.githubRepoPath, commitMessageInput.text, commitDescription.text);
            commitMessageInput.text = "";
            commitDescription.text = "";
           
        }
        catch (Exception ex)
        {
            // Handle any errors that occur during the commit
            Debug.LogError($"Commit failed: {ex.Message}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
