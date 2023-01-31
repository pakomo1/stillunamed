using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SwipeToUI : MonoBehaviour
{
    [SerializeField] private RectTransform mainPanel;
    private bool isRight = true;

    public void Swipe()
    {
        var moveLeftAmount = mainPanel.sizeDelta.x / 2;
        var moveRightAmount = -moveLeftAmount;

        if (isRight)
        {
            mainPanel.DOAnchorPos(new Vector2(moveRightAmount, 0), 0.25f);
            isRight = false;
        }
        else
        {
            mainPanel.DOAnchorPos(new Vector2(moveLeftAmount, 0), 0.25f);
            isRight=true;
        }
    }
}
