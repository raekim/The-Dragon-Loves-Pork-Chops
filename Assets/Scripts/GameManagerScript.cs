using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    private static GameManagerScript _GameManager;    // Singleton instance of GameManagerScript
    public static GameManagerScript GameManager
    {
        get { return _GameManager; }
    }

    public int meatCount;   // Amount of meat the player has
    public Text meatText;

    private void Awake()
    {
        if(_GameManager != null && _GameManager != this)    // singleton instance already here
        {
            Destroy(this.gameObject);
        }
        else
        {
            _GameManager = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateMeatText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUpMeat()
    {
        meatCount++;
        UpdateMeatText();
    }

    private void UpdateMeatText()
    {
        meatText.text = "Meat Count: " + meatCount;
    }

    public bool IsMeatAvailable()
    {
        return meatCount > 0;
    }

    public void LoseMeat(int amount)
    {
        meatCount -= amount;
        if (meatCount < 0) meatCount = 0;
        UpdateMeatText();
    }
}
