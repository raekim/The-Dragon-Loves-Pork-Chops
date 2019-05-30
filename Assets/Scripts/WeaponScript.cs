using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponScript : MonoBehaviour
{
    public float attackRate = 0.2f;
    public bool attackOn = false;
    public abstract void Attack();
}
