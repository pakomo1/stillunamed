using Octokit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RepoButtonTemplateManager : MonoBehaviour,IPointerClickHandler
{
    public UnityEvent<Repository, GameObject> onLeftClick;
    public UnityEvent<Repository, GameObject> onRightClick;
    public UnityEvent<Repository, GameObject> onMiddleClick;
    private RepositoryData repositoryData;


    private void Start()
    {
        repositoryData = GetComponent<RepositoryData>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            onLeftClick.Invoke(repositoryData.repository, gameObject);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            onRightClick.Invoke(repositoryData.repository, gameObject);
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            onMiddleClick.Invoke(repositoryData.repository, gameObject);
        }
    }
}
