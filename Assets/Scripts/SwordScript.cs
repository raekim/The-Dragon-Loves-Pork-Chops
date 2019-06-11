using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : WeaponScript
{
    [Tooltip("(int) The damage of the sword")]
    public int swordDamage = 20;
    [Tooltip("(float) Attack cooltime")]
    public float attackCooltime = 1f;
    private Animator Animator;

    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    public override void Attack()
    {
        // Attack with sowrd
        // attackOn variable become true and false as the sword goes through swinging animation
        Animator.SetTrigger("AttackTrigger");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (attackOn && collision.gameObject.tag == "Pig")
        {
            // Deal damage to the pig
            collision.gameObject.GetComponent<CharacterScript>().Hit(swordDamage);
        }
    }
}
