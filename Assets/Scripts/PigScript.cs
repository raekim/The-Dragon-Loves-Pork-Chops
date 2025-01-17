﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigScript : CharacterScript
{
    // Pig Stats
    [Tooltip("(float) The propability of calling Stand()")]
    public float standProb;
    [Tooltip("(float) The propability of calling Charge()")]
    public float chargeProb;
    [Tooltip("(float) The propability of calling Walk()")]
    public float walkProb;
    [Tooltip("(float) The speed at which the pig walks")]
    public float walkingSpeed;
    private float chargingSpeed;
    private int pigSize;    // The size of the pig [1-3]
    

    Rigidbody2D rgb2d;
    public enum BehaviorState {Standing = 1, Walking, Charging};
    private float elapsedTime;  // Time elapsed since the last state change
    private float changeSateTime;   // Time needed for the pig to change its state
    private BehaviorState curState;
    private Vector2 walkDirection;
    public GameObject playerObject;
    public GameObject meatObject;

    // Static variables
    private static readonly int maxPigSize = 3;
    public float minChangeStateTime = 2f;
    public float maxChangeStateTime = 6f;
    public static float sizeMultiple = 3;  // Multiplier of the pig size

    new private void Awake()
    {
        base.Awake();
        // Get needed components
        rgb2d = GetComponent<Rigidbody2D>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        chargingSpeed = walkingSpeed * 2f;
        playerObject = GameManagerScript.GameManager.PlayerObj;
        // Start the pig
        StartNewPig();
    }

    // Update is called once per frame
    void Update()
    {
        // Change state if needed
        if (elapsedTime >= changeSateTime)
        {
            elapsedTime = 0;
            ChangeToNextState();
        }

        elapsedTime += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        switch (curState)
        {
            case BehaviorState.Standing:
                Stand();
                break;
            case BehaviorState.Walking:
                Walk();
                break;
            case BehaviorState.Charging:
                // Face the direction of player
                var velocity = transform.position - playerObject.transform.position;
                FaceMovingDirection(velocity);
                // Charge at the player
                Charge((Vector2)playerObject.transform.position);
                break;
            default:
                break;
        }
    }

    public void StartNewPig()
    {
        InitPigStats();
        ChangeToNextState();
    }

    // Initialize a new pig stats
    public void InitPigStats()
    {
        elapsedTime = 0;

        // Determine pig size
        pigSize = Random.Range(1, maxPigSize + 1);   // Random size [1-3]

        // Set Health amount accordingly
        health = 100*pigSize;
        healthBar.SetMaxHealth(health);
        healthBar.UpdateHealthBarFilling();
        healthBar.transform.localScale = new Vector3(0.5f * pigSize, 0.1f, 1);
    }

    private float GetNextChangeStateTime()
    {
        float nextChangeStateTime = Random.Range(minChangeStateTime, maxChangeStateTime);
        return nextChangeStateTime;
    }

    // Get the next state according to probabilities
    public BehaviorState GetNextState()
    {
        BehaviorState nextState;
        float f = Random.Range(0f, 1f); // random percentage

        if (f <= standProb)
        {
            nextState = BehaviorState.Standing;
        }
        else if (f <= standProb + walkProb)
        {
            nextState = BehaviorState.Walking;
        }
        else
        {
            nextState = BehaviorState.Charging;
        }

        return nextState;
    }

    private void ChangeToNextState()
    {
        // Reset current Animation Trigger
        switch (curState)
        {
            case BehaviorState.Standing:
                animator.ResetTrigger("StandTrigger");
                break;
            case BehaviorState.Walking:
                animator.ResetTrigger("WalkTrigger");
                break;
            case BehaviorState.Charging:
                animator.ResetTrigger("ChargeTrigger");
                break;
            default:
                break;
        }

        // Change curstate and changeStateTime
        curState = GetNextState();
        changeSateTime = GetNextChangeStateTime();

        // Execute state bahavior
        switch (curState)
        {
            case BehaviorState.Standing:
                animator.SetTrigger("StandTrigger");
                break;
            case BehaviorState.Walking:
                animator.SetTrigger("WalkTrigger");
                // Pick random direction towards wich the pig will walk
                walkDirection = Random.insideUnitCircle;
                // Face the direction of walking
                //var velocity = transform.position - (transform.position + (Vector3)walkDirection);
                FaceMovingDirection((Vector3)walkDirection);
                break;
            case BehaviorState.Charging:
                animator.SetTrigger("ChargeTrigger");
                break;
            default:
                break;
        }
    }

    // Standing idly at current position
    private void Stand()
    {
        
    }

    // Charge attack at the target game object
    private void Charge(Vector2 targetPos)
    {
        float step = chargingSpeed * Time.fixedDeltaTime;
        rgb2d.MovePosition(Vector2.MoveTowards(transform.position, targetPos, step));
    }

    // Walks in a straight line towards a random direction
    private void Walk()
    {
        transform.position += (Vector3)walkDirection * walkingSpeed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (curState == BehaviorState.Charging && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<CharacterScript>().Hit(damage);
        }
    }

    private void FaceMovingDirection(Vector3 movingVector)
    {
        if (movingVector.x > 0.01)  // facing right
        {
            spr.flipX = false;
        }
        if (movingVector.x < -0.01) // facing left
        {
            spr.flipX = true;
        }
    }

    public override void Die()
    {
        // Drop meat items according to the pig's size
        DropMeat();
        GameManagerScript.GameManager.spawner.EnqueuePig(this.gameObject);
    }

    private void DropMeat()
    {
        // Drop meat pigsize amount
        for(int i=1; i<=pigSize; ++i)
        {
            Instantiate(meatObject, transform.position + new Vector3(Random.Range(-0.1f*i, 0.1f*i), Random.Range(-0.1f * i, 0.1f * i), 0), Quaternion.identity);
        }
    }
}
