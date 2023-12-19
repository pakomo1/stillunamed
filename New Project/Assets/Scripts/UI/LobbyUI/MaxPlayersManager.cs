using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxPlayersManager : MonoBehaviour
{
    public int selectedPlayerCount;
    [SerializeField] private Button playerCountBtn8;
    [SerializeField] private Button playerCountBtn24;
    [SerializeField] private Button playerCountBtn36;

    [SerializeField] private Color32 selectedColor;
    [SerializeField] private Color32 notSelectedColor;

    void Start()
    {
        SelectButton(playerCountBtn8);
        playerCountBtn8.onClick.AddListener(() => { SelectButton(playerCountBtn8); });
        playerCountBtn24.onClick.AddListener(() => { SelectButton(playerCountBtn24); });
        playerCountBtn36.onClick.AddListener(() => { SelectButton(playerCountBtn36); });
    }

    void Update()
    {
        
    }
    private void ChangeColor(Color colorToChangeWith)
    {
       
        var selectedBtn = transform.Find($"PlayerCountBtn{selectedPlayerCount}").gameObject;
        Button btn = selectedBtn.GetComponent<Button>();

        var colors = btn.colors;
        colors.normalColor = colorToChangeWith;
        btn.colors = colors;
    }
    private void SelectButton(Button buttonToSelect)
    {
        ChangeColor(notSelectedColor);
        int newselectedPlayerNumber = int.Parse(buttonToSelect.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text);
        selectedPlayerCount = newselectedPlayerNumber;
        ChangeColor(selectedColor);
    }

}
