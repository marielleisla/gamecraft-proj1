using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    float groundAcceleration = 15;
    float maxSpeed = 7.5f;
    public float jumpForce = 750;
    public LayerMask whatIsGround;
    float moveX;
    float moveJump;
    bool facingRight = true;
    Rigidbody2D rb;
    Animator anim;
    PlayerState myState;
    PlayerState nextState;
    bool stateEnded;
    GameObject duckingMario;
    GameObject littleMario;
    GameObject superMario;
    bool super;
    bool little;

    //Awake is called before any Start function
    void Awake() {
        littleMario = GameObject.Find("Little Mario");
        superMario = GameObject.Find("Super Mario");
    }

    // Use this for initialization
    void Start () {
        rb = this.transform.root.gameObject.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        anim = this.gameObject.GetComponent<Animator>();
        myState = new Grounded(this);
        if (gameObject.name == "Super Mario")
        {
            super = true;
            little = false;
            duckingMario = GameObject.Find("Ducking Mario");
            duckingMario.SetActive(false);
            superMario.SetActive(false);
        }
        else
        {
            super = false;
            little = true;
            duckingMario = null;
        }
    }

    // Update is called once per frame
    void Update () {
        moveX = Input.GetAxis("Horizontal");
        moveJump = Input.GetAxis("Jump");
        myState.Update();
        if (Input.anyKeyDown || stateEnded)
        {
            nextState = myState.HandleInput();
        }
    }

    void FixedUpdate() {
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("vSpeed", Mathf.Abs(rb.velocity.y));
        if (moveX < 0 && facingRight)
        {
            Flip();
        }
        else if (moveX > 0 && !facingRight)
        {
            Flip();
        }
        myState.FixedUpdate();
        if (nextState != null)
        {
            stateEnded = false;
            myState.Exit();
            myState = nextState;
            nextState = null;
            myState.Enter();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = this.gameObject.transform.localScale;
        scale.x = scale.x * -1;
        this.gameObject.transform.localScale = scale;
        rb.AddForce(new Vector3(-25 * rb.velocity.x, 0));
    }

    bool CheckForGround() {
        SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
        float castHeight = mySprite.sprite.bounds.size.y / 2 + 0.25f;
        Vector3 origin = new Vector3(transform.position.x, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector3.down, castHeight, whatIsGround);
        Debug.DrawRay(origin, Vector3.down * castHeight);
        return hit.collider != null;
    }

    void Duck()
    {
        duckingMario.SetActive(true);
        duckingMario.transform.position = new Vector3(transform.position.x, duckingMario.transform.position.y);
        rb.velocity = Vector3.zero;
        if (!facingRight)
        {
            Vector3 scale = duckingMario.transform.localScale;
            scale.x = scale.x * -1;
            duckingMario.transform.localScale = scale;
        }
        this.gameObject.SetActive(false);
    }


    /* If littleMario turn into superMario.
     * If superMario, add a life. In the real game,
     * when Mario is Super, all mushrooms turn into
     * fire flowers, which give him the power to shoot
     * fireballs. But we haven't implemented this. */
    public void Grow() {
        if (little) {
            superMario.SetActive(true);
            superMario.transform.position = new Vector3(this.transform.position.x, superMario.transform.position.y);
            littleMario.SetActive(false);
        } else {
            /* Do nothing for now. We will add this functionality in
             * Project 1-3. */ 
        }
    }

    /* If littleMario, then take a life. If superMario,
     * turn into littleMario. */
    public void Shrink() {
        if (little)
        {
            /* This will interact with the UI system in Project 1-3,
             * but for now we'll just reload the scene. */
            SceneManager.LoadScene("Main Scene");
        }
        else if (super) {
            littleMario.SetActive(true);
            littleMario.transform.position = new Vector3(this.transform.position.x, littleMario.transform.position.y);
            superMario.SetActive(false);
        }
    }

    /* Anything that can collide with Mario and effect him is 
     * detected in this function. It checks the layer, gets the 
     * script that contains all object functions, and then
     * calls the needed function from the attached script.
     * For enemies it will be either HitByPlayer or HitPlayer 
     * depending on the *collider tag*, and for items it will be the
     * PickUpItem function. */
    public void OnCollisionEnter2D(Collision2D coll) {
        //A layer is stored as an int. This function gets the 
        //layer name so that we can check it against strings.
        switch (LayerMask.LayerToName(coll.gameObject.layer))
        {
            case "Item":
                Item item = coll.collider.GetComponent<Item>();
                item.PickUpItem(this);
                break;
            case "Enemy":
                /*
                 * 
                 * YOUR CODE HERE
                 * 
                 */
                break;
        }
    }

    private class Grounded : PlayerState
    {

        PlayerController controller;
        Rigidbody2D rb;
        Animator anim;
        float moveX;
        float moveJump;
        float jumpForce;
        float maxSpeed;
        float groundAcceleration;

        public Grounded(PlayerController controller) {
            this.controller = controller;
            this.rb = controller.rb;
            this.anim = controller.anim;
            this.moveX = controller.moveX;
            this.moveJump = controller.moveJump;
            this.jumpForce = controller.jumpForce;
            this.maxSpeed = controller.maxSpeed;
            this.groundAcceleration = controller.groundAcceleration;

        }

        public void Enter()
        {
            moveX = Input.GetAxis("Horizontal");
            moveJump = Input.GetAxis("Jump");
            anim.SetBool("Grounded", true);
        }

        public void Update()
        {
            moveX = Input.GetAxis("Horizontal");
            moveJump = Input.GetAxis("Jump");
            if ((Input.GetButton("Vertical") && Input.GetAxis("Vertical") < -0.01f) && controller.super)
            {
                controller.Duck();
            }
        }

        public void FixedUpdate() {
            if(Mathf.Abs(rb.velocity.magnitude) <= maxSpeed)
            {
                rb.AddForce(new Vector3(groundAcceleration * moveX, 0));
            }
            if (rb.velocity.y < -2)
            {
                controller.stateEnded = true;
            }
        }

        public void Exit()
        {
            /*Determine the animation state. If jumping, add the
             preliminary jump force. */
            if (Input.GetButton("Jump"))
            {
                rb.AddForce(new Vector3(0, moveJump * jumpForce));
                anim.SetBool("Grounded", false);
                anim.SetBool("Jumping", true);
            }
            else
            {
                anim.enabled = false;
            }

        }

        public PlayerState HandleInput()
        {
            if (Input.GetButton("Jump") || controller.stateEnded)
            {
                return new InAir(controller);
            }
            else
            {
                return null;
            }
        }
    }

    private class InAir : PlayerState
    {

        PlayerController controller;
        Rigidbody2D rb;
        Animator anim;
        float moveX;
        float moveJump;
        float jumpingTime;
        float airHorizAcceleration;
        float airJumpAcceleration;

        public InAir(PlayerController controller)
        {
            this.controller = controller;
            this.rb = controller.rb;
            this.anim = controller.anim;
            this.moveX = controller.moveX;
            this.moveJump = controller.moveJump;
            this.airHorizAcceleration = 5;
            this.airJumpAcceleration = 18;
            /* Set jumpingTime to 0 if the player
             * is falling. This way they can't add
             * any extra force. */
            if (Input.GetButton("Jump"))
            {
              jumpingTime = 1;
            }
            else
            {
              jumpingTime = 0;
            }
        }

        public void Enter()
        {
            moveJump = Input.GetAxis("Jump");
            moveX = Input.GetAxis("Horizontal");
        }

        public void Update()
        {
            moveJump = Input.GetAxis("Jump");
            moveX = Input.GetAxis("Horizontal");
        }

        public void FixedUpdate()
        {
            //Jumping timer
            jumpingTime -= Time.deltaTime;
            //Control in the air
            if (Mathf.Abs(rb.velocity.x) <= controller.maxSpeed)
            {
                rb.AddForce(new Vector3(moveX * airHorizAcceleration, 0));
            }
            if (jumpingTime >= 0)
            {
                rb.AddForce(new Vector3(0, moveJump * airJumpAcceleration));
            }
            //Continuously check that you haven't hit the ground.
            if (controller.CheckForGround())
            {
                controller.stateEnded = true;
            }
        }

        public void Exit()
        {
            anim.enabled = true;
            anim.SetBool("Jumping", false);
            rb.velocity = new Vector3(rb.velocity.x, 0);
        }

        public PlayerState HandleInput()
        {
            if (controller.stateEnded)
            {
                return new Grounded(controller);
            }
            else
            {
                return null;
            }
        }
    }

}
