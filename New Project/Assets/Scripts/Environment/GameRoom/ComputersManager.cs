using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputersManager : MonoBehaviour
{

    [SerializeField] private GameObject computerPrefab;
    [SerializeField] private GameObject room; // GameObject representing the room
    [SerializeField] private GameObject EditorPrefab; 


    private void InitializeComputers(int count)
    {
        float computerWidth = computerPrefab.GetComponent<Renderer>().bounds.size.x; // Width of a computer
        float roomWidth = room.GetComponent<BoxCollider2D>().bounds.size.x; // Width of the room
        float totalComputersWidth = count * computerWidth; // Total width of all computers
        float space = ((roomWidth - totalComputersWidth) / (count + 1)) * 0.01f; // Space between each computer

        for (int i = 0; i < count; i++)
        {
            // Calculate the position for each computer
            float xPos = room.transform.position.x - roomWidth / 2 + space * (i + 1) + computerWidth * i;
            Vector3 position = new Vector3(xPos, room.transform.position.y, room.transform.position.z);

            // Instantiate the computer at the calculated position as a child of this GameObject
            GameObject computer = Instantiate(computerPrefab, position, Quaternion.identity);
            computer.transform.SetParent(room.transform);

            // Instantiate the text editor for this computer and make it a child of the computer
            GameObject Editor = Instantiate(EditorPrefab, computer.transform);
            Editor.GetComponent<TextEditorData>().Id =i;  
            Editor.SetActive(false);
        }
    }
    private void Start()
    {
        InitializeComputers(5);
    }
}
