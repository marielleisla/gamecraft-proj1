using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour{

    protected Rigidbody2D rb;
    protected Collider2D myCollider;
    protected SpriteRenderer mySprite;
    
    public virtual void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        myCollider = GetComponent<Collider2D>();
        mySprite = transform.parent.GetComponentInChildren<SpriteRenderer>();
        /*This code ignores collisions between enemies and items, 
         * so that enemies can't pick up or run into items. */
        int itemLayer = LayerMask.NameToLayer("Item");
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(itemLayer, enemyLayer, true);
    }

    /* Use for item movement, sounds, etc. */
    public abstract void FixedUpdate();

    /* Called when item is picked up. */
    public abstract void PickUpItem(PlayerController player);
}
