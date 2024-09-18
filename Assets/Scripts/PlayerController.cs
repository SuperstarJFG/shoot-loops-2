using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEditor.SceneManagement;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject AIOverlayPrefab;
    [SerializeField] private GameObject glowPrefab;
    [SerializeField] private GameObject bigGlowPrefab;
    [SerializeField] private Sprite heart_broken_empty;
    [SerializeField] private Sprite heart_broken_full;
    [SerializeField] private Sprite heart_fixed_empty;
    [SerializeField] private Sprite heart_fixed_full;
    [SerializeField] private Sprite heart_fixed_charging;
    [SerializeField] private AudioClip lowSound;
    [SerializeField] private AudioClip healSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private GameObject controlsOverlayPrefab;

    public int playerNumber;
    public Color color { get; private set; }
    public Vector3 inputVector { private get; set; }
    private int _HP;
    public int HP 
    {
        get { return _HP; }
        private set
        {
            Debug.Log(value + " " + isHowToPlay);
            if (isHowToPlay)
            { 
                if (value >= 1 && value <= 2)
                    _HP = value;
            }
            else
            {
                if (value >= 0 && value <= 2)
                    _HP = value;
            }
        }
    }
    public GameObject arrow { get; private set; }
    public bool isAI;
    public bool isFull { get; private set; }
    public bool isHowToPlay;
    public float secondsAlive { get; private set; }

    private Rigidbody2D RB;
    private float secondsSinceLastHurt;
    private AudioSource audioSource;
    private Color darkColor
    {
        get
        {
            return new Color(color.r, color.g, color.b, 0.1f);
        }
    }
    private GameObject AIOverlay;
    private GameObject glow;
    
    void Start()
    {
        color = new Color(UnityEngine.Random.Range(0.2f, 1f), UnityEngine.Random.Range(0.2f, 1f), UnityEngine.Random.Range(0.2f, 1f));
        GetComponent<SpriteRenderer>().color = color;
        if (isAI) { 
            AIOverlay = Instantiate(AIOverlayPrefab, transform);
            if (SceneManager.GetActiveScene().name == "MainMenu")
                AIOverlay.SetActive(false);
        }
        else
            Instantiate(controlsOverlayPrefab, transform);
        glow = Instantiate(glowPrefab, transform);
        glow.GetComponent<SpriteRenderer>().color = color;

        name = "Player " + playerNumber;
        RB = GetComponent<Rigidbody2D>();
        isFull = true;
        HP = 2;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(DoLowHPEfects());
    }

    void Update()
    {
        switch (HP)
        {
            case 0:
                Darken();
                Destroy(arrow);
                break;
            case 1:
                GetComponent<SpriteRenderer>().sprite = (isFull) ? heart_broken_full : heart_broken_empty;
                glow.SetActive(false);
                break;
            case 2:
                Lighten();
                GetComponent<SpriteRenderer>().sprite = (isFull) ? heart_fixed_full : heart_fixed_empty;
                glow.SetActive(true);
                break;
        }

        secondsSinceLastHurt += Time.deltaTime;
        secondsAlive += Time.deltaTime;

        if (secondsAlive < 3f)
        {
            return;
        }

        if (Input.GetAxisRaw("Fire " + playerNumber) > 0 && !isAI)
        {
            TryFire();
        }

        Debug.DrawLine(transform.position, transform.position + inputVector, Color.white);
    }

    void FixedUpdate()
    {
        if (secondsAlive < 3f)
        {
            return;
        }

        if (!isAI)
            inputVector = new Vector3(Input.GetAxisRaw("Horizontal " + playerNumber), Input.GetAxisRaw("Vertical " + playerNumber));
        if (HP > 0)
            RB.MovePosition((Vector3) RB.position + inputVector * speed * Time.deltaTime);
    }

    IEnumerator DoLowHPEfects()
    {
        while (true)
        { 
            if (HP == 1)
            {
                Darken();
                audioSource.PlayOneShot(lowSound);
            }
            yield return new WaitForSeconds(2f/15f);
            if (HP == 1)
            {
                Lighten();
            }
            yield return new WaitForSeconds(6f/15f);
        }
    }

    void Darken()
    {
        GetComponent<SpriteRenderer>().color = darkColor;
        if (isAI)
            AIOverlay.GetComponentInChildren<SpriteRenderer>().color = Color.grey;
    }

    void Lighten()
    {
        GetComponent<SpriteRenderer>().color = color;
        if (isAI)
            AIOverlay.GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }

    public void Hurt()
    {
        if (secondsSinceLastHurt > 0.5f)
        {
            HP--;
            audioSource.PlayOneShot(hurtSound);
            secondsSinceLastHurt = 0f;
        }
    }

    public void Heal()
    {
        HP++;
        audioSource.PlayOneShot(healSound);
    }

    public void Fill()
    {
        isFull = true;
    }

    public void TryFire()
    {
        if (isFull && HP > 0 && secondsAlive > 3f)
        {
            arrow = Instantiate(arrowPrefab, transform.position, transform.rotation);
            arrow.GetComponent<ArrowController>().owner = gameObject;
            isFull = false;
            audioSource.PlayOneShot(fireSound);
        }
    }
}
