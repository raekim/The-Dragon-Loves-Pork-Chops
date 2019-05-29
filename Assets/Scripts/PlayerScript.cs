using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// transform.position += (Vector3)walkDirection * walkingSpeed * Time.fixedDeltaTime;
public class PlayerScript : MonoBehaviour
{
    // Player Stats
    [Tooltip("(float) The speed at which the player walks")]
    public float walkingSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
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
