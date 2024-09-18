using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Slider humanPlayerCountSlider;
    [SerializeField] private Slider AIDifficultySlider;
    [SerializeField] private GameObject countdownPrefab;
    [SerializeField] private GameObject gameOverTextPrefab;
    [SerializeField] private GameObject menuManager;

    [Range(0, 4)] private int humanPlayerCount;
    [Range(1, 9)] private int AIDifficulty;
    private List<GameObject> players = new();
    private HashSet<GameObject> livingPlayers = new();
    private enum Scene
    {
        mainMenu,
        howToPlay,
        play
    }
    private Scene scene;
    private AudioSource audioSource;
    private float secondsSinceGameOver;

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
        ManageAISlider();
        if (scene == Scene.play)
            ManageGameOver();
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

            audioSource.Play();
            Instantiate(countdownPrefab);
        }

        if (scene == Scene.howToPlay)
        {
            menuManager.GetComponent<MenuController>().EnableQuitAndRestartButtons();
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

        GetComponentInChildren<Canvas>().gameObject.SetActive(false);
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
        PC.isHowToPlay = (scene == Scene.howToPlay);

        players.Add(player);
    }

    void ManageAISlider()
    {
        if (AIDifficultySlider == null)
            return;
        if (humanPlayerCountSlider.value <= 1f && scene == Scene.play)
            AIDifficultySlider.gameObject.SetActive(true);
        else
            AIDifficultySlider.gameObject.SetActive(false);
    }

    void ManageGameOver()
    {
        // manage livingPlayers
        foreach (GameObject player in players)
        {
            if (player.GetComponent<PlayerController>().HP > 0)
            {
                livingPlayers.Add(player);
            }
            else
            {
                livingPlayers.Remove(player);
            }
        }

        // manage secondsSinceGameOver
        if (livingPlayers.Count <= 1)
        {
            secondsSinceGameOver += Time.deltaTime;
        }
        else
        {
            secondsSinceGameOver = 0f;
        }

        // display winner
        if (secondsSinceGameOver > 3f)
        {
            Time.timeScale = 0f;
            TextMeshPro gameOverText = Instantiate(gameOverTextPrefab, transform, true).GetComponent<TextMeshPro>();
            menuManager.GetComponent<MenuController>().EnableQuitAndRestartButtons();
            if (livingPlayers.Count == 1)
            {
                PlayerController PC = livingPlayers.First().GetComponent<PlayerController>();
                gameOverText.text = "P" + PC.playerNumber + " WINS";
                gameOverText.color = livingPlayers.First().GetComponent<PlayerController>().color;
            }
            else
            { 
                gameOverText.text = "DRAW";
            }
        }
    }
}