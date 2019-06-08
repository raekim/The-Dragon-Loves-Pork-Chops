using System.Collections;
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

    // Static variables
    private static readonly int maxPigSize = 3;
    public float minChangeStateTime = 2f;
    public float maxChangeStateTime = 6f;
    public static float sizeMultiple = 3;  // Multiplier of the pig size



    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        // Get needed components
        rgb2d = GetComponent<Rigidbody2D>();

        // Start the pig
        chargingSpeed = walkingSpeed * 2f;
        InitPigStats();
        ChangeToNextState();
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

    // Initialize a new pig stats
    public void InitPigStats()
    {
        elapsedTime = 0;
        // Determine pig size
        pigSize = Random.Range(1, maxPigSize + 1);   // Random size [1-3]
        health = 100*pigSize;
        healthBar.SetMaxHealth(health);
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
                var velocity = transform.position - (transform.position + (Vector3)walkDirection);
                FaceMovingDirection(velocity);
                break;
            case BehaviorState.Charging:
                animator.SetTrigger("ChargeTrigger");
                Debug.Log("CHarge");
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
        Debug.Log(movingVector);
        if (movingVector.x > 0.01)  // facing right
        {
            spr.flipX = false;
        }
        if (movingVector.x < -0.01) // facing left
        {
            spr.flipX = true;
        }
    }
}
