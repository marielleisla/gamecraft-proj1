using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMushroom : Item {

    Vector3 activatedPosition;
    Vector3 currentSpeed = new Vector3(5, 0);
    float timeToHide = 0.2f;
    /*Magic Mushroom needs to activate first by rising up.
     * Not all items need to have an activation. */
    bool activated;

    public override void Start()
    {
        base.Start();
        /* When Mario hits a block he will also run into the
         * trigger zone of Magic Mushroom's collider. */
        myCollider.isTrigger = true;
        /* When unactivated, the Magic Mushroom will not be 
         * effected by gravity. */
        rb.isKinematic = true;
        activatedPosition = transform.position;
        activatedPosition.y = activatedPosition.y + 1;
    }

    public override void FixedUpdate()
    {
        if (activated)
        {
            if (rb.velocity.magnitude <= 0.1f)
            {
                currentSpeed.x *= -1;
                rb.velocity = new Vector3(currentSpeed.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector3(currentSpeed.x, rb.velocity.y);
            }
        }
    }

    /* This coroutine will run every physics step until the 'yield'
     * statement. For more information on Coroutines, check the Unity
     * manual. */
    IEnumerator Activate()
    {
        while (transform.position.y < (activatedPosition.y - 0.01f))
        {
            rb.velocity = new Vector3(0, 2f);
            yield return null;
        }
        rb.velocity = Vector3.zero;
        myCollider.isTrigger = false;
        rb.isKinematic = false;
        rb.velocity = currentSpeed;
        activated = true;
        yield break;
    }

    /* This coroutine briefly hides the sprite so that it isn't visible
     * when Mario hits and moves the block. */
    IEnumerator ShowAndHide()
    {
        mySprite.enabled = false;
        yield return new WaitForSeconds(timeToHide);
        mySprite.enabled = true;
        yield break;
    }

    public override void PickUpItem(PlayerController player)
    {
        player.Grow();
        Destroy(gameObject);
    }

    /* When the player enters the trigger collider, begin activation. */
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine("Activate");
            StartCoroutine("ShowAndHide");
        }
    }

}
