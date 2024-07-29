using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField][Range(1, 4)] private int playerCount;

    private GameObject player;

    void Start()
    {
        for (int i = 0; i < System.Math.Max(playerCount, 2); i++)
        {
            player = Instantiate(playerPrefab, transform.GetChild(i).position, transform.GetChild(i).rotation);

            player.GetComponent<PlayerController>().playerNumber = i + 1;
        }
    }

    void Update()
    {

    }
}