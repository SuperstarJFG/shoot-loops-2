using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlsOverlayController : MonoBehaviour
{
    [SerializeField] private TextMeshPro moveText;
    [SerializeField] private TextMeshPro shootText;

    private int playerNumber;

    private string[,] inputStrings = { 
        { "↑", "←", "↓", "→", "RSHIFT" },
        { "W", "A", "S", "D", "TAB" },
        { "I", "J", "K", "L", "Y" },
        { "8", "4", "5", "6", "\\" }
    };

    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        
        playerNumber = GetComponentInParent<PlayerController>().playerNumber;
        moveText.text = 
            inputStrings[playerNumber - 1, 0] + "\n" 
            + inputStrings[playerNumber - 1, 1] + "     " + inputStrings[playerNumber - 1, 3] + "\n" 
            + inputStrings[playerNumber - 1, 2];
    }

    void Update()
    {
        if (GetComponentInParent<PlayerController>().isFull)
            shootText.text = inputStrings[playerNumber - 1, 4];
        else
            shootText.text = "";
    }
}
