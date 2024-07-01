using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField][Range(1, 4)] private int playerCount;

    private GameObject[] players = new GameObject[4];
    private Color[] playerColors = new Color[4];

    void Start()
    {
        for (int i = 0; i < playerCount; i++)
        {
            players[i] = Instantiate(playerPrefab);

            players[i].name = "Player " + (i + 1);

            playerColors[i] = new Color(Random.Range(.1f, 1f), Random.Range(.1f, 1f), Random.Range(.1f, 1f));
            players[i].GetComponent<SpriteRenderer>().color = playerColors[i];

            players[i].GetComponent<PlayerController>().inputNameHorizontal = "Horizontal " + (i + 1);
            players[i].GetComponent<PlayerController>().inputNameVertical = "Vertical " + (i + 1);
            players[i].GetComponent<PlayerController>().inputNameFire = "Fire " + (i + 1);
        }

        if (playerCount <= 2)
        {
            players[0].transform.position = new Vector3(2.5f, 2.5f, 0);
            players[1].transform.position = new Vector3(-2.5f, -2.5f, 0);
            players[1].transform.Rotate(0, 0, 180);
        }
    }

    void Update()
    {

    }
}