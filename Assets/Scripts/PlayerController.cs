using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float fireRate;
    [SerializeField] private GameObject arrowPrefab;

    public string inputNameHorizontal;
    public string inputNameVertical;
    public string inputNameFire;

    private Rigidbody2D rb;
    private Vector2 inputVector;
    private Vector2 velocityVector;

    private bool full = true;
    private int hp = 2;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (full && Input.GetAxisRaw(inputNameFire) > 0)
        {
            Instantiate(arrowPrefab, transform);
            full = false;
        }
    }

    void FixedUpdate()
    {
        inputVector = new Vector2(Input.GetAxisRaw(inputNameHorizontal), Input.GetAxisRaw(inputNameVertical));

        rb.MovePosition(rb.position + inputVector * speed * Time.fixedDeltaTime);

    }
}
