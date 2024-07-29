using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Looper : MonoBehaviour
{
    private float verticalBound = 5f;
    private float horizontalBound = 5f * 16f/9f;
    private List<Vector3> childPositions = new List<Vector3>();
    private Vector3 newPosition;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        newPosition = transform.position;
        if (transform.position.y > verticalBound)
            newPosition.y = -verticalBound + 0.01f;
        if (transform.position.y < -verticalBound)
            newPosition.y = verticalBound - 0.01f;
        if (transform.position.x > horizontalBound)
            newPosition.x = -horizontalBound + 0.01f;
        if (transform.position.x < -horizontalBound)
            newPosition.x = horizontalBound - 0.01f;
        transform.position = newPosition;
    }
}
