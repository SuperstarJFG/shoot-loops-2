using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField][Range(0, 4)] private int humanPlayerCount;
    [SerializeField][Range(1, 9)] private int AIDifficulty;

    private List<GameObject> players = new();

    void Start()
    {
        switch (humanPlayerCount)
        {
            case 0:
                SpawnPlayer(1, true);
                SpawnPlayer(2, true);
                break;
            case 1:
                SpawnPlayer(1, false);
                SpawnPlayer(2, true);
                break;
            case >= 2:
                for (int i = 1; i < humanPlayerCount + 1; i++)
                    SpawnPlayer(i, false);
                break;
        }
        
        if (players[0].GetComponent<PlayerController>().isAI)
            players[0].GetComponent<AIController>().enemy = players[1];
        if (players[1].GetComponent<PlayerController>().isAI)
            players[1].GetComponent<AIController>().enemy = players[0];

        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }

    void SpawnPlayer(int playerNumber, bool isAI)
    {
        Transform spawnPoint = transform.GetChild(playerNumber - 1);
        GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        PlayerController PC = player.GetComponent<PlayerController>();
        AIController AIC = player.GetComponent<AIController>();

        PC.playerNumber = playerNumber;
        if (isAI)
        {
            PC.isAI = true;
            AIC.enabled = true;
            AIC.difficulty = AIDifficulty;
        }

        players.Add(player);
    }
}