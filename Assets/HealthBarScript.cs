using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    [Tooltip("The game object this health bar is attached to")]
    public GameObject character;
    [Tooltip("The offset of the position of this health bar")]
    public Vector3 healthbarOffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FollowCharacter();
    }

    // Position the health bar so that it follows the character
    private void FollowCharacter()
    {
        transform.position = Camera.main.WorldToScreenPoint(character.transform.position) + healthbarOffset;
    }
}
