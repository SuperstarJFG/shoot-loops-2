using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Looper : MonoBehaviour
{
    private float verticalBound = 5f;
    private float horizontalBound = 5f * 16f/9f;
    private List<Vector3> childPositions = new List<Vector3>();
    void Start()
    {
        
    }

    void Update()
    {
        foreach (Transform child in transform)
            childPositions.Add(child.position);
        if (transform.position.y > verticalBound)
            transform.position = new Vector3(transform.position.x, -verticalBound + 0.01f, transform.position.z);
        if (transform.position.y < -verticalBound)
            transform.position = new Vector3(transform.position.x, verticalBound - 0.01f, transform.position.z);
        if (transform.position.x > horizontalBound)
            transform.position = new Vector3(-horizontalBound + 0.01f, transform.position.y, transform.position.z);
        if (transform.position.x < -horizontalBound)
            transform.position = new Vector3(horizontalBound - 0.01f, transform.position.y, transform.position.z);
        foreach (Transform child in transform)
            child.position = childPositions[0];
        childPositions.Clear();
    }
}
