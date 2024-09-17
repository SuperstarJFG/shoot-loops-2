using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float speed;

    public GameObject owner;

    private Rigidbody2D rb;
    private float timeAlive;
    private PlayerController ownerPC;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ownerPC = owner.GetComponent<PlayerController>();
        GetComponent<SpriteRenderer>().color = ownerPC.color;
        timeAlive = 0;
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
        if (collision.name.Contains("Player"))
        {
            if (collision.gameObject == owner)
            {
                if (timeAlive > 0.2f)
                {
                    ownerPC.Heal();
                    ownerPC.Fill();
                    Destroy(gameObject);
                }
            }
            else
            {
                collision.GetComponent<PlayerController>().Hurt();
            }
        }
    }
}