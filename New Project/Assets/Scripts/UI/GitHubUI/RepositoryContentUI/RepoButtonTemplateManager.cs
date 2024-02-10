using Octokit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RepoButtonTemplateManager : MonoBehaviour,IPointerClickHandler
{
    public UnityEvent<Repository> onLeftClick;
    public UnityEvent<Repository> onRightClick;
    public UnityEvent<Repository> onMiddleClick;
    private RepositoryData repositoryData;


    private void Start()
    {
        repositoryData = GetComponent<RepositoryData>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            onLeftClick.Invoke(repositoryData.repository);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            onRightClick.Invoke(repositoryData.repository);
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            onMiddleClick.Invoke(repositoryData.repository);
        }
    }
}
