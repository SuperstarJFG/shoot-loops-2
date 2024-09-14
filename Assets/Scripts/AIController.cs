using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class AIController : MonoBehaviour
{
    public GameObject enemy { private get; set; }
    public int difficulty { private get; set; }

    private Vector3 positionOfEnemy { get { return enemy.transform.position; } }
    private Vector3 positionOfEnemyArrow
    {
        get
        {
            if (enemy.GetComponent<PlayerController>().arrow == null)
                return Vector3.positiveInfinity;
            else
                return enemy.GetComponent<PlayerController>().arrow.transform.position;
        }
    }
    private Vector3 positionInFrontOfEnemyArrow;
    private Vector3 targetVector;
    private GameObject targetObject
    {
        set
        {
            if (value != null)
                targetVector = value.transform.position;
        }
    }
    private enum State
    {
        idle,
        dodging,
        lowHP,
        targeting,
        movingRandomly
    }
    private State _state;
    private State state
    {
        get { return _state; }
        set
        {
            if (value == _state)
                return;
            Debug.Log("changing state to " + value);
            StartCoroutine(ChangeStateAfterSeconds(value, RNG(0.0f, 0.0f)));
        }

    }
    private bool isWaitingToFire;
    private PlayerController PC;
    private Color debugLineColor;

    void Start()
    {
        PC = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (RNG(0,1) < 0.001f)
        {
            state = State.movingRandomly;
            StartCoroutine(MoveRandomly());
        }
        if (state == State.movingRandomly)
            return;

        // check if low hp
        if (PC.HP == 1)
            state = State.lowHP;

        // dodge if needed
        positionInFrontOfEnemyArrow = positionOfEnemyArrow + transform.up * 2;
        if (Vector3.Distance(positionInFrontOfEnemyArrow, transform.position) < (3 + difficulty/5f))
            state = State.dodging;
        else if (PC.HP == 2)
            state = State.targeting;
        // start targeting if full hp and not dodging

        switch (state)
        {
            case State.idle:
                targetObject = gameObject;
                break;
            case State.dodging:
                targetVector = transform.position + transform.right*RNG(0,1) + transform.up*RNG(0,1);
                debugLineColor = Color.yellow;
                break;
            case State.lowHP:
                if (PC.isFull)
                    TryFire();
                else
                    targetObject = PC.arrow;
                debugLineColor = Color.blue;
                break;
            case State.targeting:
                if (PC.isFull)
                {
                    Debug.Log("targeting and full. difficulty " + difficulty);
                    targetVector = positionOfEnemy + transform.up * (1 + (5f/difficulty));
                    if (Vector3.Distance(targetVector, transform.position) < 0.5f)
                        TryFire();
                }
                else
                    targetObject = PC.arrow;
                debugLineColor = Color.red;
                break;
            default:
                break;
        }

        Follow(targetVector);

        Debug.DrawLine(transform.position, targetVector, debugLineColor);
        //Debug.Log("targetVector " + targetVector);
        Debug.DrawLine(transform.position, positionInFrontOfEnemyArrow, Color.green);
        //Debug.Log("positionInFrontOfEnemyArrow " + positionInFrontOfEnemyArrow);
        //Debug.Log("state " + state);

    }

    void FixedUpdate()
    {
        
    }

    void Follow(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        if (Vector3.Distance(target, transform.position) > 0.3f)
        {
            PC.inputVector = Vector3Int.RoundToInt(direction.normalized);
        }
        else {
            PC.inputVector = Vector3.zero;
            //Debug.Log("reached target");
        }
    }

    IEnumerator ChangeStateAfterSeconds(State state, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _state = state;
    }
    IEnumerator TryFireAfterSeconds(float seconds)
    {
        isWaitingToFire = true;
        Debug.Log("waiting seconds " + seconds);
        yield return new WaitForSeconds(seconds);
        Debug.Log("firing");
        PC.TryFire();
        isWaitingToFire = false;
    }
    void TryFire()
    {
        if (!isWaitingToFire)
            StartCoroutine(TryFireAfterSeconds(RNG(0, 1f/difficulty)));
    }

    float RNG(float minInclusive, float maxInclusive)
    {
        return UnityEngine.Random.Range(minInclusive, maxInclusive);
    }
    IEnumerator MoveRandomly()
    {
        if (difficulty > RNG(1, 10))
            TryFire();
        Follow(100 * new Vector3(RNG(-1, 1), RNG(-1, 1)));
        yield return new WaitForSeconds(3);//RNG(0, 1));
        state = State.idle;
    }
}