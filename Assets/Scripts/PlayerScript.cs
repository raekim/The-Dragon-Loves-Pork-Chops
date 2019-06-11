using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// transform.position += (Vector3)walkDirection * walkingSpeed * Time.fixedDeltaTime;
public class PlayerScript : CharacterScript
{
    // Player Stats
    [Tooltip("(float) The speed at which the player walks")]
    public float walkingSpeed;
    public GameObject weapon;
    private WeaponScript weaponScript;

    private float curTime = 0;
    private bool facingRight = true;

    public GameObject ThrowObject;   // object to throw
    public float ThrowPower = 20f;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        SetCharacterDirection();
        weaponScript = weapon.GetComponent<WeaponScript>();
    }

    // Update is called once per frame
    void Update()
    {
        FaceMousePoint();

        if (Input.GetMouseButton(0) && curTime >= weaponScript.attackRate)  // Left Mouse click attacks
        {
            curTime = 0;
            weaponScript.Attack();
        }
        if (Input.GetMouseButtonDown(1))    // Right Mouse click throws meat
        {
            ThrowMeat();
        }

        curTime += Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void SetCharacterDirection()
    {
        var delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        facingRight = (delta.x <= 0) ? true : false;
    }

    // Let the player face left/right based on the mouse point location
    private void FaceMousePoint()
    {
        // using mousePosition and player's transform (on orthographic camera view)
        var delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if (delta.x >= 0 && !facingRight)   // face right
        {
            transform.localScale = new Vector3(-size, size, 0);
            facingRight = true;
        }
        else if (delta.x < 0 && facingRight) {  // face left
            transform.localScale = new Vector3(size, size, 0);
            facingRight = false;
        }
    }

    // Move the character
    private void MoveCharacter()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        animator.SetBool("isWalking", movement.magnitude != 0);
        movement.Normalize();
        transform.position += (Vector3)movement * walkingSpeed * Time.fixedDeltaTime;
    }

    public override void Die()
    {
        Destroy(this);
    }

    private void ThrowMeat()
    {
        // No meat to throw
        if (!GameManagerScript.GameManager.IsMeatAvailable())
            return;

        // Throw meat in a straight line
        var meat = Instantiate(ThrowObject, transform.position, Quaternion.identity);
        Vector2 forceVector;
        forceVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        forceVector.Normalize();
        forceVector *= ThrowPower;
        meat.GetComponent<Rigidbody2D>().AddForce(forceVector, ForceMode2D.Impulse);

        // meatCount decreases by 1
        GameManagerScript.GameManager.LoseMeat(1);
    }
}
