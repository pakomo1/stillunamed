using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowViewerBtnManager : MonoBehaviour
{
    [SerializeField] private Button showViewerBtn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       showViewerBtn.gameObject.SetActive(!PlayerManager.LocalPlayer.isPlayerInteracting());
    }
}
