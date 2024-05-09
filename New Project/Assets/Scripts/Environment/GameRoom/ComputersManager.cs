using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ComputersManager : NetworkBehaviour
{

    [SerializeField] private GameObject computerPrefab;
    [SerializeField] private GameObject room; // GameObject representing the room
    [SerializeField] private GameObject EditorPrefab;
  //  [SerializeField] private TextEditorManager textEditorManager;

    public static event EventHandler OnCompuerInitialized;

    public static NetworkVariable<bool>ComputersInitialized = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    private void InitializeComputers(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject computer = Instantiate(computerPrefab);
            //spawn the computer
            NetworkObject networkObjectEditor = computer.GetComponent<NetworkObject>();
            if (networkObjectEditor != null)
            {
                networkObjectEditor.Spawn();
            }
            computer.transform.SetParent(transform);
        }
        ComputersInitialized.Value = true;
        OnCompuerInitialized?.Invoke(this, EventArgs.Empty);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            StartCoroutine(InitializeComputersCoroutine(5));
        }
    }

    private IEnumerator InitializeComputersCoroutine(int count)
    {
        // Wait for the next frame to ensure that TextEditorManager.Instance has been initialized
        yield return null;

            InitializeComputers(count);
        }
}
