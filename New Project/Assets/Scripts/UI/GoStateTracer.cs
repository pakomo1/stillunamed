using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoStateTracer : MonoBehaviour
{
    public GameObject target; // The GameObject to track
    public event Action<bool> OnActiveStateChanged;

    private bool lastActiveState;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target not set for ActiveStateTracker");
            return;
        }

        lastActiveState = target.activeSelf;
    }

    private void Update()
    {
        if (target == null)
            return;

        bool currentActiveState = target.activeSelf;
        if (currentActiveState != lastActiveState)
        {
            OnActiveStateChanged?.Invoke(currentActiveState);
            lastActiveState = currentActiveState;
        }
    }
}
