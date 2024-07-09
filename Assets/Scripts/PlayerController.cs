using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Sprite heart_broken_empty;
    [SerializeField] private Sprite heart_broken_full;
    [SerializeField] private Sprite heart_fixed_empty;
    [SerializeField] private Sprite heart_fixed_full;
    [SerializeField] private Sprite heart_fixed_charging;
    [SerializeField] private AudioClip lowSound;

    public string inputNameHorizontal;
    public string inputNameVertical;
    public string inputNameFire;
    public Color playerColor;
    [Range(0,2)] public int hp;
    public bool full;

    private Rigidbody2D rb;
    private Vector2 inputVector;
    private Vector2 velocityVector;
    private AudioSource audioSource;
    private float timeSinceLastFire;
    private bool charging;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().color = playerColor;
        full = true;
        hp = 2;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(LowHP());
    }

    void Update()
    {
        if (timeSinceLastFire < 1)
            charging = true;
        else
            charging = false;

        if (full && Input.GetAxisRaw(inputNameFire) > 0 && hp > 0 && !charging)
        {
            Instantiate(arrowPrefab, transform);
            full = false;
            timeSinceLastFire = 0;
        }
        timeSinceLastFire += Time.deltaTime;

        switch (hp)
        {
            case <1:
                hp = 0;
                Darken(true);
                break;
            case 1:
                GetComponent<SpriteRenderer>().sprite = (full) ? heart_broken_full : heart_broken_empty;
                break;
            case >1:
                hp = 2;
                Darken(false);
                GetComponent<SpriteRenderer>().sprite = (full) ? heart_fixed_full : heart_fixed_empty;
                if (charging && full)
                    GetComponent<SpriteRenderer>().sprite = heart_fixed_charging;
                break;
        }

        if (Input.GetAxisRaw("R") > 0)
        {
            Restart();
        }
    }

    void FixedUpdate()
    {
        inputVector = new Vector2(Input.GetAxisRaw(inputNameHorizontal), Input.GetAxisRaw(inputNameVertical));
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
                Darken(true);
                audioSource.PlayOneShot(lowSound);
            }
            yield return new WaitForSeconds(2f/15f);
            if (hp == 1)
            {
                Darken(false);
            }
            yield return new WaitForSeconds(6f/15f);
        }
    }

    void Darken(bool darkening)
    {
        Color temp = GetComponent<SpriteRenderer>().color;
        temp.a = (darkening) ? 0.1f : 1f;
        GetComponent<SpriteRenderer>().color = temp;
    }
}
