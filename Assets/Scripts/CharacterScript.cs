﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    public int health;
    public int damage;
    public HealthBarScript healthBar;
    protected Animator animator;
    public SpriteRenderer spr;
    public float size;

    public void Start()
    {
        animator = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
    }

    // Take damage
    public void Hit(int damage)
    {
        health -= damage;
        healthBar.UpdateHealthBarFilling();
        if (health <= 0)
        {
            // Character dies
            Destroy(gameObject);
        }
    }

}
