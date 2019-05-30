using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponScript : MonoBehaviour
{
    public bool attackOn = false;
    public abstract void Attack();
}
