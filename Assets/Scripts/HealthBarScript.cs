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
    private int characterMaxHealth;
    private CharacterScript CharacterScript;

    void Awake()
    {
        CharacterScript = character.GetComponent<CharacterScript>();
        healthBarImage = GetComponentsInChildren<Image>()[1];
    }

    // Start is called before the first frame update
    private void Start()
    {
        SetMaxHealth(CharacterScript.health);
        UpdateHealthBarFilling();
    }

    public void SetMaxHealth(int health)
    {
        characterMaxHealth = health;  
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
        healthBarImage.fillAmount = (float)CharacterScript.health / characterMaxHealth;
    }
}
