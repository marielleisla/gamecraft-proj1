using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy {

    float walkingSpeed = 5;
    Vector3 currentSpeed;

    public override void Start() {
        base.Start();
        currentSpeed = new Vector3(walkingSpeed, 0);
        rb.velocity = currentSpeed;
    }

	// Update is called once per frame
	public override void FixedUpdate () {
        if (dead)
        {
            if (timeToDeath <= 0)
            {
                Destroy(this.gameObject);
            }
            timeToDeath -= Time.deltaTime;
        }
        else
        {
            if (rb.velocity.magnitude <= 0.1f)
            {
                currentSpeed.x *= -1;
                rb.velocity = currentSpeed;
            }
            else
            {
                rb.velocity = currentSpeed;
            }
        }
    }

    /* When a Goomba is hit by the player, it dies: entering the 
     * dead (squashed) animation state, settings its velocity to 0 
     * and becoming kinematic, and destroying its colliders. */
    public override void HitByPlayer(PlayerController player)
    {
        /*
         * 
         * YOUR CODE HERE
         * 
         */ 
        rb.isKinematic = true;
    }

    /* When a Goomba hits the player, the player shrinks. See
     PlayerController.Shrink(). */
    public override void HitPlayer(PlayerController player)
    {
        /*
         * 
         * YOUR CODE HERE
         * 
         */
    }

}
