using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Octokit;
using UnityEngine.UI;
using TMPro;

public class IssuesTemplate : MonoBehaviour
{
    [SerializeField] private Button issueButtonTemplate;
    [SerializeField] private Sprite openIssueImage;
    [SerializeField] private Sprite closeIssueImage;
    public void GenerateIssues(IReadOnlyList<Issue> issues)
    {
        foreach (var issue in issues)
        {
            Button issueButton = Instantiate(issueButtonTemplate,transform);
            
            issueButton.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = issue.Title;

            //state


            if (issue.State == ItemState.Open)
            {
                issueButton.transform.Find("State").GetComponent<Image>().sprite = openIssueImage;
            }
            else if (issue.State == ItemState.Closed)
            {
                issueButton.transform.Find("State").GetComponent<Image>().sprite = closeIssueImage;
            }

            issueButton.transform.Find("NumberOfComents").GetComponent<TextMeshProUGUI>().text = issue.Comments.ToString();
            issueButton.gameObject.SetActive(true);
        }
    }
}
