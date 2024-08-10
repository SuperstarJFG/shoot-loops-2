using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public GameObject enemy { get; set; }

    private Vector3 targetVector;
    //private GameObject targetObject
    //{
    //    set
    //    {
    //        targetVector = value.transform.position + transform.up * 5;
    //    }
    //}
    private float distanceToTarget
    {
        get
        {
            return Vector3.Distance(targetVector, transform.position);
        }
    }
    private enum State
    {
        idle,
        dodging,
        lowHP,
        targeting,
        firing
    }
    private State _state;
    private State state
    {
        get { return _state; }
        set
        {
            StartCoroutine(ChangeStateAfterWaitingSeconds(value, 1.0f));
        }

    }    
    private Vector3 positionInFrontOfEnemyArrow;
    private PlayerController PC;

    void Start()
    {
        PC = GetComponent<PlayerController>();
    }

    void Update()
    {        
        // check if low hp
        if (PC.hp == 1)
            state = State.lowHP;

        // dodge if needed
        positionInFrontOfEnemyArrow = Vector3.positiveInfinity;
        if (enemy.GetComponent<PlayerController>().arrow != null)
            positionInFrontOfEnemyArrow = enemy.GetComponent<PlayerController>().arrow.transform.position + transform.up * 5;
        if (Vector3.Distance(positionInFrontOfEnemyArrow, transform.position) < 2f)
            state = State.dodging;
        else if (PC.hp == 2)
            state = State.targeting;
        // start targeting if full hp and not dodging

        switch (state)
        {
            case State.idle:
                targetVector = transform.position;
                break;
            case State.dodging:
                targetVector = transform.position + (transform.up + transform.right) * 5;
                PC.TryFire();
                break;
            case State.lowHP:
                if (PC.isFull)
                    PC.TryFire();
                else
                    targetVector = PC.arrow.transform.position;
                break;
            case State.targeting:
                if (PC.isFull)
                    targetVector = enemy.transform.position + transform.up * 5;
                else
                    targetVector = PC.arrow.transform.position;
                if (distanceToTarget < 0.5f)
                    PC.TryFire();
                break;
            default:
                break;
        }

        Follow(targetVector);

        Debug.DrawLine(transform.position, targetVector, Color.red);
        //Debug.Log("targetVector " + targetVector);
        Debug.DrawLine(transform.position, positionInFrontOfEnemyArrow, Color.green);
        //Debug.Log("positionInFrontOfEnemyArrow " + positionInFrontOfEnemyArrow);
        Debug.Log("state " + state);

    }

    void FixedUpdate()
    {
        
    }

    void Follow(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        float distance = Vector3.Distance(target, transform.position);
        if (distance > 0.1f)
        {
            PC.inputVector = Vector3Int.RoundToInt(direction.normalized);
        }
        else {
            PC.inputVector = Vector3.zero;
        }
    }

    IEnumerator ChangeStateAfterWaitingSeconds(State state, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _state = state;
    }
}