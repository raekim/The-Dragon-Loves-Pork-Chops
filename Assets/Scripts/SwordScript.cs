using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : WeaponScript
{
    [Tooltip("(int) The damage of the sword")]
    public int swordDamage = 20;
    private Animator Animator;

    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    public override void Attack()
    {
        // Attack with sowrd
        Animator.SetTrigger("AttackTrigger");
        // attackOn variable become true and false as the sword goes through swinging animation
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (attackOn && collision.gameObject.tag == "Pig")
        {
            // Deal damage to the pig
            collision.gameObject.GetComponent<PigScript>().Hit(swordDamage);
        }
    }
}
