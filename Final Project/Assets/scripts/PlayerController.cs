using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private BoxCollider collider;
    private int mode;
    private int stand = 0;
    private int move = 1;
    private int attack = 2;
    private int jumping = 3;
    private int attackOverride = 0;
    private string input = "";
    public float speed, lean, jumpForce, runForce, landOffset;
    public int direction; // -1 for left, 1 for right
    public Vector3 standCenter;// = new Vector3((float)-.2, (float)-.4, 0);
    public Vector3 standSize;// = new Vector3((float)1.3, (float)4.3, (float).8);
    public Vector3 runCenter;// = new Vector3((float)-.45, (float)-.87, 0);
    public Vector3 runSize;// = new Vector3((float)2.2, (float)3.4, (float).8);
    public float horiz;
    public float vert;
    public Transform fireball;

    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
        collider = this.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        mode = getKey();

        if (mode == attack || attackOverride == 1)
        {
            special();
        }
        else if (mode == move)
        {
            run();
        } else if (mode == jumping) 
        {
            jump();
        }
        else if (mode == stand)
        {
            idle();
        }
    }

    void jump() {
        //if havent started jump yet, start animation
        if (!animator.GetBool("Jumping")) {
            animator.SetTrigger("Jump");
        }

        if (isPressingRunKeys()) {
            run();
        }

    }

    void takeAir() {
        //add velocity
        if (Physics.Raycast(transform.position, Vector3.down, landOffset)) {
            rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    void stillInAir() {

        if (Physics.Raycast(transform.position, Vector3.down, landOffset)) {
            animator.enabled = true;
        } else {
            animator.enabled = false;
        }
    }

    void idle()
    {
        if (animator.GetBool("RunningLeftRight"))
        { //if running
            animator.SetBool("RunningLeftRight", false);

            BoxCollider bc = GetComponent<BoxCollider>();
            bc.center = standCenter;
            bc.size = standSize;
        }

        this.transform.localEulerAngles = new Vector3((float)0, (float)0, (float)0);
    }

    //change direction player is moving
    void FlipAnimation()
    {
        direction *= -1; //switch direction player is facing

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    //for moving player left or right
    Vector3 moveHoriz(float horiz)
    {
        if (!animator.GetBool("RunningLeftRight"))
        { //if not already running
            animator.SetBool("RunningLeftRight", true);
            BoxCollider bc = GetComponent<BoxCollider>();
            bc.center = runCenter;
            bc.size = runSize;
        }

        Vector3 ret;

        //if direction is not the same as horizontal input
        if ((direction < 0) != (horiz < 0))
        {
            FlipAnimation();
        }

        if (direction < 0)
        {
            ret = Vector3.left * speed * Time.deltaTime;
            //ret = Vector3.left;
        }
        else {
            ret = Vector3.right * speed * Time.deltaTime;
            //ret = Vector3.right;
        }

        return ret;
    }

    //for moving player in and out
    Vector3 moveInOut(float vert)
    {
        if (!animator.GetBool("RunningLeftRight"))
        {   //if not already running
            animator.SetBool("RunningLeftRight", true);

            BoxCollider bc = GetComponent<BoxCollider>();
            bc.center = runCenter;
            bc.size = runSize;
        }

        Vector3 ret;

        //move in or out and rotate
        if (vert < 0)
        {
            this.transform.localEulerAngles = new Vector3((float)-lean, (float)0, (float)0);
            ret = Vector3.back * speed * Time.deltaTime;
            //ret = Vector3.back;
        }
        else {
            this.transform.localEulerAngles = new Vector3((float)lean, (float)0, (float)0);
            ret = Vector3.forward * speed * Time.deltaTime;
            //ret = Vector3.forward;
        }

        return ret;
    }

    void run()
    {
        Vector3 toMove = transform.position;

        if (vert != 0)
        {   //going in or out
           toMove += moveInOut(vert);
        } else {
            this.transform.localEulerAngles = new Vector3((float)0, (float)0, (float)0);
        }

        if (horiz != 0)
        {   //going right or left
            toMove += moveHoriz(horiz);
        }

        //rb.AddForce(toMove * runForce, ForceMode.Impulse);
        rb.MovePosition(toMove);
    }

    void executeMove(string input)
    {
        //print("executing " + input);

        if (input == "dr")
        {
            Vector3 spawnPos = GetComponent<Transform>().position;
            spawnPos.x += direction * 2;

            Instantiate(fireball, spawnPos, transform.rotation);

            //print("FIREBALL");
        }
        else
        {
            //print("NOT A MOVE");
        }
    }

    void special()
    {
        if (!animator.GetBool("SpecialAttack"))
        {
            animator.SetBool("SpecialAttack", true);
            //print("starting attack animation");
            return;
        }
        //print("checking attack input");
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            input += "u";
            //print(input);
        } 
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            input += "d";
            //print(input);
        }    
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            input += "l";
            //print(input);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            input += "r";
            //print(input);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            attackOverride = 0;
            animator.SetBool("SpecialAttack", false);
            executeMove(input);
            input = "";
            //print("ending attack");
        }
    }

    int getKey()
    {
        //if starting special attack or already entering attack
        if (Input.GetKeyDown(KeyCode.Space) || attackOverride == 1)
        {
            attackOverride = 1;
            //print("starting attack");
            return attack;
        }

        //if starting jump or already jumping
        if (Input.GetKeyDown(KeyCode.CapsLock) || animator.GetBool("Jumping")) {
            return jumping;
        }

        //if trying to run
        if ( isPressingRunKeys() && !animator.GetBool("Jumping"))
        {
            return move;
        }

        return stand;
    }

    bool isPressingRunKeys() {
        bool ret = false;

        if (Input.GetKey("w")) {
            vert = 1;
            ret = true;
        } else if (Input.GetKey("s")) {
            vert = -1;
            ret = true;
        } else {
            vert = 0;
        }

        if (Input.GetKey("d")) {
            horiz = 1;
            ret = true;
        } else if (Input.GetKey("a")) {
            horiz = -1;
            ret = true;
        } else {
            horiz = 0;
        }

        return ret;
    }

}