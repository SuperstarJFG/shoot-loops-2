using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private float secondsHoldingQuit;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
            DisableChildren();
    }

    void Update()
    {
        if (Input.GetAxisRaw("R") > 0 && Input.GetAxisRaw("Left Shift") > 0)
        {
            Restart();
        }
        
        if (Input.GetAxisRaw("Q") > 0 && Input.GetAxisRaw("Left Shift") > 0)
        {
            secondsHoldingQuit += Time.deltaTime;
        }
        else
        {
            secondsHoldingQuit = 0;
        }

        if (secondsHoldingQuit > 3f)
        {
            Quit();
        }
    }

    public void Restart()
    {
        Debug.Log("restart");

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Debug.Log("quit");

        SceneManager.LoadScene("MainMenu");
    }
    
    public void HowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    public void Play()
    {
        SceneManager.LoadScene("Play");
    }

    void DisableChildren()
    {
       foreach (Transform child in transform)
       {
            child.gameObject.SetActive(false);
       }
    }
}