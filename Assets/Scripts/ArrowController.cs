using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private AudioClip healSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip fireSound;

    private Rigidbody2D rb;
    private float timeAlive;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().color = transform.parent.GetComponent<PlayerController>().playerColor;
        timeAlive = 0;
        audioSource = transform.parent.GetComponent<AudioSource>();
        audioSource.PlayOneShot(fireSound);
    }

    void Update()
    {
        timeAlive += Time.deltaTime;
    }

    void FixedUpdate()
    {
        Vector3 temp = rb.position;
        rb.MovePosition(temp - transform.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            if (collision.gameObject == transform.parent.gameObject)
            {
                if (timeAlive > 0.2f) {
                    Hit(true);
                }
            }
            else
            {
                Hit(false);
            }
        }
    }

    void Hit(bool self)
    {
        if (self)
        {
            transform.parent.GetComponent<PlayerController>().hp++;
            audioSource.PlayOneShot(healSound);
        }
        else
        {
            transform.parent.GetComponent<PlayerController>().hp--;
            audioSource.PlayOneShot(hurtSound);
        }
        Destroy(gameObject);
        transform.parent.GetComponent<PlayerController>().full = true;
    }
}