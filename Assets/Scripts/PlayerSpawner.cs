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
    //[SerializeField] private GameObject[] spawnPoints = new GameObject[4];

    private List<GameObject> players = new List<GameObject>();
    private Transform spawnPoint;

    void Start()
    {
        for (int i = 1; i < transform.childCount + 1; i++)
        {
            spawnPoint = transform.GetChild(i - 1);

            if (i <= playerCount)
            {
                spawnPlayer(spawnPoint, i, false);
            }
            else if (i <= 2)
            {
                spawnPlayer(spawnPoint, i, true);
            }

            Destroy(spawnPoint.gameObject);
        }
    }

    void Update()
    {

    }

    void spawnPlayer(Transform spawnPoint, int playerNumber, bool isAI)
    {
        GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        PlayerController PC = player.GetComponent<PlayerController>();
        PC.playerNumber = playerNumber;
        if (isAI)
        {
            PC.isAI = true;
            player.AddComponent<AIController>();
        }
    }
}