using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public GameObject target;

    private PlayerController PC;

    void Start()
    {
        PC = GetComponent<PlayerController>();
        PC.inputVector = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
        PC.inputVector.x = 0;
        PC.inputVector.y = 0;
    }

    void Update()
    {
        PC.TryFire();
        //if (PC.hp < 2)
        //{
        //    target = PC.arrow;
        //}
    }

    void FixedUpdate()
    {
        
    }
}