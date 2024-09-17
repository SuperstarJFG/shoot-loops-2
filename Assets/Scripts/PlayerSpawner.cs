using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Slider humanPlayerCountSlider;
    [SerializeField] private Slider AIDifficultySlider;
    [SerializeField] private TextMeshProUGUI countdownText;

    [Range(0, 4)] private int humanPlayerCount;
    [Range(1, 9)] private int AIDifficulty;
    
    private List<GameObject> players = new();
    private enum Scene
    {
        mainMenu,
        howToPlay,
        play
    }
    private Scene scene;
    private AudioSource audioSource;
    private float countdown;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        switch (SceneManager.GetActiveScene().name)
        {
            case "MainMenu":
                scene = Scene.mainMenu;
                SpawnPlayers();
                break;
            case "HowToPlay":
                scene = Scene.howToPlay;
                break;
            case "Play":
                scene = Scene.play;
                break;
        }
    }

    void Update()
    {
        if (AIDifficultySlider == null)
            return;
        if (humanPlayerCountSlider.value < 2f && scene == Scene.play)
            AIDifficultySlider.gameObject.SetActive(true);
        else
            AIDifficultySlider.gameObject.SetActive(false);

        countdown -= Time.deltaTime;
        switch (countdown)
        {
            case > 0f:
                countdownText.text = countdown.ToString("0.0");
                break;
            case > -1f:
                countdownText.text = "GO";
                break;
            default:
                countdownText.text = "";
                break;
        }
    }

    public void SpawnPlayers()
    {
        if (scene == Scene.mainMenu)
        { 
            humanPlayerCount = 0;
            AIDifficulty = 5;
        }
        else
        { 
            humanPlayerCount = (int) humanPlayerCountSlider.value;
            AIDifficulty = (int) AIDifficultySlider.value;
            countdown = 3f;
        }

        if (scene == Scene.howToPlay)
        {
            for (int i = 1; i < humanPlayerCount + 1; i++)
                SpawnPlayer(i, false);
        }
        else
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
        }

        audioSource.Play();


        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(true);
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
        PC.isHowToPlay = scene == Scene.howToPlay;

        players.Add(player);
    }
}