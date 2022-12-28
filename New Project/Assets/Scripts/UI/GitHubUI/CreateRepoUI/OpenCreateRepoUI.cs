using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCreateRepoUI : MonoBehaviour
{
    [SerializeField] private GameObject createRepositoryUI;

    public void OpenUI()
    {
        createRepositoryUI.SetActive(true);
    }
}
