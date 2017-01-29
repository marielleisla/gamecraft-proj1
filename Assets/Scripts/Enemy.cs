using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

    protected Rigidbody2D rb;
    protected Animator anim;
    /* An enemy may have multiple colliders. */
    protected Collider2D[] myColliders;
    protected float timeToDeath = 0.5f;
    protected bool dead = false;

    // Use this for initialization
    public virtual void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        anim = GetComponent<Animator>();
        myColliders = GetComponentsInChildren<Collider2D>();
    }

    public abstract void FixedUpdate();

    /* Called BY the Player Controller (passing itself in) when 
     * the Player collides with an Enemy's HitByPlayer collider. */
    public abstract void HitByPlayer(PlayerController player);

    /* Called BY the Player Controller (passing itself in) when the 
     * Player collides with an Enemy's HitPlayer collider. */
    public abstract void HitPlayer(PlayerController player);
}
