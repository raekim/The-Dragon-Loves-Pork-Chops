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

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        SetCharacterDirection();
        weaponScript = weapon.GetComponent<WeaponScript>();
    }

    // Update is called once per frame
    void Update()
    {
        FaceMousePoint();

        // Use GetButtonDown later
        if (Input.GetButton("Fire1") && curTime >= weaponScript.attackRate)
        {
            curTime = 0;
            weaponScript.Attack();
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
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }
        else if (delta.x < 0 && facingRight) {  // face left
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }
    }

    // Move the character
    private void MoveCharacter()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        movement.Normalize();
        transform.position += (Vector3)movement * walkingSpeed * Time.fixedDeltaTime;
    }
}
