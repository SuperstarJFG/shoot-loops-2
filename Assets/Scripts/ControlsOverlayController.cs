using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlsOverlayController : MonoBehaviour
{
    [SerializeField] private TextMeshPro moveText;
    [SerializeField] private TextMeshPro shootText;
    [SerializeField] private TextMeshPro playerNumberText;

    private int playerNumber;
    private string[,] inputStrings = { 
        { "↑", "←", "↓", "→", "RSHIFT" },
        { "W", "A", "S", "D", "TAB" },
        { "I", "J", "K", "L", "Y" },
        { "8", "4", "5", "6", "\\" }
    };
    private PlayerController PC;

    void Start()
    {
        PC = GetComponentInParent<PlayerController>();

        transform.rotation = Quaternion.Euler(0, 0, 0);
        
        playerNumber = PC.playerNumber;
        moveText.text = 
            inputStrings[playerNumber - 1, 0] + "\n" 
            + inputStrings[playerNumber - 1, 1] + "     " + inputStrings[playerNumber - 1, 3] + "\n" 
            + inputStrings[playerNumber - 1, 2];

        playerNumberText.text = "P" + playerNumber.ToString();
    }

    void Update()
    {
        if (!PC.isHowToPlay && PC.secondsAlive > 3f)
            Destroy(gameObject);

        if (PC.isFull)
            shootText.text = inputStrings[playerNumber - 1, 4];
        else
            shootText.text = "";
    }
}
