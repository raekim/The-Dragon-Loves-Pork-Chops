using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // The player picks up meat
        if (collision.gameObject.tag == "Player")
        {
            GameManagerScript.GameManager.PickUpMeat();
            Destroy(this.gameObject);
        }
    }
}
