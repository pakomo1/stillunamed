using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator anim;
    private string currentState;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ChangeAnimationState(string _newState)
    {
        if (currentState == _newState) return;
        anim.Play(_newState);
        currentState = _newState;
    }
}
