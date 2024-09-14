using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEditor.SceneManagement;
using UnityEngine;

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

    public int playerNumber { get; set; }
    public Color color { get; private set; }
    public Vector3 inputVector { private get; set; }
    private int _HP;
    public int HP 
    {
        get { return _HP; }
        private set
        {
            if (value < 0 || value > 2)
                return;
            _HP = value;
        }
    }
    public GameObject arrow { get; private set; }
    public bool isAI;
    public bool isFull { get; private set; }

    private Rigidbody2D RB;
    //private bool isCharging;
    //private float timeSinceLastFire;
    private float timeSinceLastHurt;
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

        name = "Player " + playerNumber;
        RB = GetComponent<Rigidbody2D>();
        isFull = true;
        HP = 2;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(DoLowHPEfects());

        if (isAI)
            AIOverlay = Instantiate(AIOverlayPrefab, transform);
        glow = Instantiate(glowPrefab, transform);
        glow.GetComponent<SpriteRenderer>().color = color;
    }

    void Update()
    {
        //isCharging = (timeSinceLastFire < 1);

        if (Input.GetAxisRaw("Fire " + playerNumber) > 0 && !isAI)
        {
            TryFire();
        }
        //timeSinceLastFire += Time.deltaTime;
        timeSinceLastHurt += Time.deltaTime;

        switch (HP)
        {
            case 0:
                Darken();
                Destroy(arrow);
                //Time.timeScale = 0f;
                break;
            case 1:
                GetComponent<SpriteRenderer>().sprite = (isFull) ? heart_broken_full : heart_broken_empty;
                glow.SetActive(false);
                break;
            case 2:
                Lighten();
                GetComponent<SpriteRenderer>().sprite = (isFull) ? heart_fixed_full : heart_fixed_empty;
                glow.SetActive(true);
                //if (isFull && isCharging)
                //    GetComponent<SpriteRenderer>().sprite = heart_fixed_charging;
                break;
        }

        if (Input.GetAxisRaw("R") > 0 && Input.GetAxisRaw("Left Shift") > 0)
        {
            Restart();
        }

        Debug.DrawLine(transform.position, transform.position + inputVector, Color.white);
    }

    void FixedUpdate()
    {
        if (!isAI)
            inputVector = new Vector3(Input.GetAxisRaw("Horizontal " + playerNumber), Input.GetAxisRaw("Vertical " + playerNumber));
        if (HP > 0)
            RB.MovePosition((Vector3) RB.position + inputVector * speed * Time.deltaTime);
    }

    void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
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
        if (timeSinceLastHurt > 0.5f)
        {
            HP--;
            audioSource.PlayOneShot(hurtSound);
            timeSinceLastHurt = 0f;
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
        if (isFull && HP > 0)
        {
            arrow = Instantiate(arrowPrefab, transform.position, transform.rotation);
            arrow.GetComponent<ArrowController>().owner = gameObject;
            isFull = false;
            //timeSinceLastFire = 0;
            audioSource.PlayOneShot(fireSound);
        }
    }
}
