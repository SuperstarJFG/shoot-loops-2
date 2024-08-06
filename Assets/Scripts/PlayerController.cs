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
    [SerializeField] private Sprite heart_broken_empty;
    [SerializeField] private Sprite heart_broken_full;
    [SerializeField] private Sprite heart_fixed_empty;
    [SerializeField] private Sprite heart_fixed_full;
    [SerializeField] private Sprite heart_fixed_charging;
    [SerializeField] private AudioClip lowSound;
    [SerializeField] private AudioClip healSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip fireSound;

    [Range(1,4)] public int playerNumber;
    public Color color;
    public Vector2 inputVector;
    public bool isAI;

    private Rigidbody2D rb;
    private Vector2 velocityVector;
    [Range(0,2)] private int hp;
    private bool full;
    private bool charging;
    private float timeSinceLastFire;
    private AudioSource audioSource;
    private Color darkColor;
    private AIController AIController;
    private GameObject AIOverlay;
    
    void Start()
    {
        color = new Color(UnityEngine.Random.Range(.1f, 1f), UnityEngine.Random.Range(.1f, 1f), UnityEngine.Random.Range(.1f, 1f));
        GetComponent<SpriteRenderer>().color = color;
        darkColor = color;
        darkColor.a = 0.1f;

        name = "Player " + playerNumber;
        rb = GetComponent<Rigidbody2D>();
        full = true;
        hp = 2;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(LowHP());

        if (isAI)
            AIOverlay = Instantiate(AIOverlayPrefab, transform);
    }

    void Update()
    {
        charging = (timeSinceLastFire < 1);

        if (Input.GetAxisRaw("Fire " + playerNumber) > 0 && !isAI)
        {
            TryFire();
        }
        timeSinceLastFire += Time.deltaTime;

        switch (hp)
        {
            case <1:
                hp = 0;
                Darken();
                break;
            case 1:
                GetComponent<SpriteRenderer>().sprite = (full) ? heart_broken_full : heart_broken_empty;
                break;
            case >1:
                hp = 2;
                Lighten();
                GetComponent<SpriteRenderer>().sprite = (full) ? heart_fixed_full : heart_fixed_empty;
                if (charging && full)
                    GetComponent<SpriteRenderer>().sprite = heart_fixed_charging;
                break;
        }

        if (Input.GetAxisRaw("R") > 0 && Input.GetAxisRaw("Left Shift") > 0)
        {
            Restart();
        }
    }

    void FixedUpdate()
    {
        if (!isAI)
            inputVector = new Vector2(Input.GetAxisRaw("Horizontal " + playerNumber), Input.GetAxisRaw("Vertical " + playerNumber));
        if (hp > 0)
            rb.MovePosition(rb.position + inputVector * speed * Time.deltaTime);
    }

    void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    IEnumerator LowHP()
    {
        while (true)
        { 
            if (hp == 1)
            {
                Darken();
                audioSource.PlayOneShot(lowSound);
            }
            yield return new WaitForSeconds(2f/15f);
            if (hp == 1)
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
        hp--;
        audioSource.PlayOneShot(hurtSound);
    }

    public void Heal()
    {
        hp++;
        audioSource.PlayOneShot(healSound);
    }

    public void Fill()
    {
        full = true;
    }

    public void TryFire()
    {
        GameObject arrow;

        if (full && hp > 0 && !charging)
        {
            arrow = Instantiate(arrowPrefab, transform.position, transform.rotation);
            arrow.GetComponent<ArrowController>().owner = gameObject;
            full = false;
            timeSinceLastFire = 0;
            audioSource.PlayOneShot(fireSound);
        }
    }
}
