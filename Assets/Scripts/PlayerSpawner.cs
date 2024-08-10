using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField][Range(0, 4)] private int playerCount;

    private Transform spawnPoint;
    private List<GameObject> players = new List<GameObject>();

    void Start()
    {
        for (int i = 1; i < transform.childCount + 1; i++)
        {
            spawnPoint = transform.GetChild(i - 1);

            if (i <= playerCount)
            {
                players.Add(SpawnPlayer(spawnPoint, i, false));
            }
            else if (i <= 2)
            {
                players.Add(SpawnPlayer(spawnPoint, i, true));
            }

            Destroy(spawnPoint.gameObject);
        }
        if (players[0].GetComponent<PlayerController>().isAI)
            players[0].GetComponent<AIController>().enemy = players[1];
        if (players[1].GetComponent<PlayerController>().isAI)
            players[1].GetComponent<AIController>().enemy = players[0];
    }

    void Update()
    {

    }

    GameObject SpawnPlayer(Transform spawnPoint, int playerNumber, bool isAI)
    {
        GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        PlayerController PC = player.GetComponent<PlayerController>();
        AIController AIC = player.GetComponent<AIController>();
        PC.playerNumber = playerNumber;
        if (isAI)
        {
            PC.isAI = true;
            AIC.enabled = true;
        }
        return player;
    }
}