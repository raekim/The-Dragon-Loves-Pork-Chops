using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingMeatScript : MonoBehaviour
{
    private float currTime;
    [Tooltip("Life duration of this object (in seconds)")]
    public float duration = 5f;  // Meat dissapears after this time

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        if(currTime >= duration)
        {
            Destroy(this.gameObject);
        }
    }
}
