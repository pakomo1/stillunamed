using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Octokit;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class IssuesTemplate : MonoBehaviour
{
    [SerializeField] private Button issueButtonTemplate;
    [SerializeField] private Sprite openIssueImage;
    [SerializeField] private Sprite closeIssueImage;
    public void GenerateIssues(NetworkList<SerializedIssues> issues)
    {
        ClearIssues();
        foreach (var issue in issues)
        {
            Button issueButton = Instantiate(issueButtonTemplate,transform);
            
            issueButton.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = issue.Title.ToString();

            //state


            if (issue.State.ToString().ToLower() == "open")
            {
                issueButton.transform.Find("State").GetComponent<Image>().sprite = openIssueImage;
            }
            else if (issue.State.ToString().ToLower() == "closed")
            {
                issueButton.transform.Find("State").GetComponent<Image>().sprite = closeIssueImage;
            }

            issueButton.transform.Find("NumberOfComents").GetComponent<TextMeshProUGUI>().text = issue.Comments.ToString();
            issueButton.gameObject.SetActive(true);
        }
    }
    //Clears the issues 
    public void ClearIssues()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
