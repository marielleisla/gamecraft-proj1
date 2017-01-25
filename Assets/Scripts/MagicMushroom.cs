﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMushroom : Item {

    Vector3 activatedPosition;
    bool finishedActivation = false;
    bool onFloor = false;
    Vector3 currentSpeed = new Vector3(5, 0);
    float timeToShow = 0.2f;

    public override void Start()
    {
        base.Start();
        activatedPosition = transform.position;
        activatedPosition.y = activatedPosition.y + 1;
    }

    public override void ItemBehavior()
    {
        //Use coroutine for rising up;
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

    IEnumerator Activate()
    {
        while (transform.position.y < (activatedPosition.y - 0.01f))
        {
            rb.velocity = new Vector3(0, 2f);
            yield return null;
        }
        Debug.Log("When does this all get called?");
        rb.velocity = Vector3.zero;
        myCollider.isTrigger = false;
        finishedActivation = true;
        rb.isKinematic = false;
        rb.velocity = currentSpeed;
        activated = true;
        yield break;
    }

    IEnumerator ShowAndHide()
    {
        mySprite.enabled = false;
        yield return new WaitForSeconds(0.2f);
        mySprite.enabled = true;
        yield break;
    }

    public override void PickUpItem(PlayerController player)
    {
        player.Grow();
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine("Activate");
            StartCoroutine("ShowAndHide");
        }
    }

    void OnCollisionStay2D(Collision2D coll) {
        onFloor = true;
    }

    void OnCollisionExit2D(Collision2D coll) {
        onFloor = false;
        //rb.AddForce(new Vector3(-15, -200));
    }

}
