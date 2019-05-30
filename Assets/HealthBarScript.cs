using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    [Tooltip("The game object this health bar is attached to")]
    public GameObject character;
    [Tooltip("The offset of the position of this health bar")]
    public Vector3 healthbarOffset;
    private Image healthBarImage;
    public int characterMaxHealth;

    // Start is called before the first frame update
    void Start()
    {
        healthBarImage = GetComponentsInChildren<Image>()[1];   // get healthbar filling image component
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

    // Update healthbar filling amount according to character's HP
    public void UpdateHealthBarFilling()
    {
        
        healthBarImage.fillAmount = (float)character.GetComponent<PigScript>().health / characterMaxHealth;
        Debug.Log(healthBarImage.fillAmount);
    }
}
