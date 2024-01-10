using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Octokit;
using UnityEngine.UI;
using TMPro;

public class IssuesTemplate : MonoBehaviour
{
    [SerializeField] private Button issueButtonTemplate;
    public void GenerateIssues(IReadOnlyList<Issue> issues)
    {
        foreach (var issue in issues)
        {
            Button issueButton = Instantiate(issueButtonTemplate,transform);
            
            issueButton.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = issue.Title;

            var state = issueButton.transform.Find("State").GetComponent<Image>();
            //set the right image for issue state
            issueButton.transform.Find("NumberOfComents").GetComponent<TextMeshProUGUI>().text = issue.Comments.ToString();

            issueButton.gameObject.SetActive(true);
        }
    }
}
