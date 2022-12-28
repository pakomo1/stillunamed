using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CreateRepository : MonoBehaviour
{
    [SerializeField] private ToggleGroup visibilityOptions;
    [SerializeField] private Toggle readmeFileInitBtn;

    public void SendRequestToCreateRepository()
    {
        Toggle chosenVisibilityOption = visibilityOptions.ActiveToggles().FirstOrDefault();
        print(chosenVisibilityOption.name);
        print(readmeFileInitBtn.isOn);
    }
}
