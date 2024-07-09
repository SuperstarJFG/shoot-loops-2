using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField][Range(1, 4)] private int playerCount;

    private GameObject[] players = new GameObject[4];
    private UnityEngine.Color thisColor;

    void Start()
    {
        for (int i = 0; i < System.Math.Max(playerCount, 2); i++)
        {
            players[i] = Instantiate(playerPrefab);

            players[i].name = "Player " + (i + 1);

            thisColor = new UnityEngine.Color(Random.Range(.1f, 1f), Random.Range(.1f, 1f), Random.Range(.1f, 1f));
            players[i].GetComponent<PlayerController>().playerColor = thisColor;

            players[i].GetComponent<PlayerController>().inputNameHorizontal = "Horizontal " + (i + 1);
            players[i].GetComponent<PlayerController>().inputNameVertical = "Vertical " + (i + 1);
            players[i].GetComponent<PlayerController>().inputNameFire = "Fire " + (i + 1);
        }

        switch (playerCount)
        {
            case <= 2:
                players[0].transform.position = new Vector3(2.5f, 2.5f, 0);
                players[1].transform.position = new Vector3(-2.5f, -2.5f, 0);
                players[1].transform.Rotate(0, 0, 180);
                break;
            case 3:
                players[0].transform.position = new Vector3(-2.5f, 0, 0);
                players[1].transform.position = new Vector3(0f, 0, 0);
                players[2].transform.position = new Vector3(2.5f, 0, 0);
                break;
            case 4:
                players[0].transform.position = new Vector3(2.5f, 2.5f, 0);
                players[2].transform.position = new Vector3(-2.5f, 2.5f, 0);

                players[1].transform.position = new Vector3(-2.5f, -2.5f, 0);
                players[3].transform.position = new Vector3(2.5f, -2.5f, 0);
                players[1].transform.Rotate(0, 0, 180);
                players[3].transform.Rotate(0, 0, 180);
                break;
        }
    }

    void Update()
    {

    }
}