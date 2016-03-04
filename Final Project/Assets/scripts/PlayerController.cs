using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private int mode;
    private int stand = 0;
    private int move = 1;
    private int attack = 2;
    private int attackOverride = 0;
    private string input = "";
    public float speed, lean;
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
        }
        else if (mode == stand)
        {
            idle();
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
    void moveHoriz(float horiz, Vector3 toMove)
    {
        if (!animator.GetBool("RunningLeftRight"))
        { //if not already running
            animator.SetBool("RunningLeftRight", true);
            BoxCollider bc = GetComponent<BoxCollider>();
            bc.center = runCenter;
            bc.size = runSize;
        }

        //if direction is not the same as horizontal input
        if ((direction < 0) != (horiz < 0))
        {
            FlipAnimation();
        }

        if (direction < 0)
        {
            //transform.Translate(Vector3.left * speed * Time.deltaTime, transform.parent);
            //rb.AddForce(Vector3.left * speed * Time.deltaTime);
            //rb.MovePosition(transform.position + (Vector3.left * speed * Time.deltaTime));
            toMove += Vector3.left * speed * Time.deltaTime;
        }
        else {
            //transform.Translate(Vector3.right * speed * Time.deltaTime, transform.parent);
            //rb.AddForce(Vector3.right * speed * Time.deltaTime);
            toMove += Vector3.right * speed * Time.deltaTime;
        }

        rb.MovePosition(toMove);
    }

    //for moving player in and out
    void moveInOut(float vert, float horiz)
    {
        if (!animator.GetBool("RunningLeftRight"))
        { //if not already running
            animator.SetBool("RunningLeftRight", true);

            BoxCollider bc = GetComponent<BoxCollider>();
            bc.center = runCenter;
            bc.size = runSize;
        }

        Vector3 toMove;

        //move in or out and rotate
        if (vert < 0)
        {
            this.transform.localEulerAngles = new Vector3((float)-lean, (float)0, (float)0);
            //transform.Translate(Vector3.back * speed * Time.deltaTime, transform.parent);
            //rb.AddForce(Vector3.back * speed * Time.deltaTime);
            toMove = transform.position + (Vector3.back * speed * Time.deltaTime);
        }
        else {
            this.transform.localEulerAngles = new Vector3((float)lean, (float)0, (float)0);
            //transform.Translate(Vector3.forward * speed * Time.deltaTime, transform.parent);
            //rb.AddForce(Vector3.forward * speed * Time.deltaTime);
            toMove = transform.position + (Vector3.forward * speed * Time.deltaTime);
        }

        //if also moving left or right
        if (horiz != 0)
        {
            moveHoriz(horiz, toMove);
        }
        else {
            rb.MovePosition(toMove);
        }
    }

    void run()
    {
        if (vert != 0)
        { //going in or out
            moveInOut(vert, horiz);
        }
        else if (horiz != 0)
        { //going right or left
            this.transform.localEulerAngles = new Vector3((float)0, (float)0, (float)0);
            moveHoriz(horiz, transform.position);
        }
    }

    void executeMove(string input)
    {
        print("executing " + input);

        if (input == "dr")
        {
            Vector3 spawnPos = GetComponent<Transform>().position;
            spawnPos.x += direction * 2;

            Instantiate(fireball, spawnPos, transform.rotation);

            print("FIREBALL");
        }
        else
        {
            print("NOT A MOVE");
        }
    }

    void special()
    {
        if (!animator.GetBool("SpecialAttack"))
        {
            animator.SetBool("SpecialAttack", true);
            print("starting attack animation");
            return;
        }
        print("checking attack input");
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            input += "u";
            print(input);
        } 
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            input += "d";
            print(input);
        }    
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            input += "l";
            print(input);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            input += "r";
            print(input);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            attackOverride = 0;
            animator.SetBool("SpecialAttack", false);
            executeMove(input);
            input = "";
            print("ending attack");
        }
    }

    int getKey()
    {
        if (Input.GetKeyDown(KeyCode.Space) || attackOverride == 1)
        {
            attackOverride = 1;
            print("starting attack");
            return attack;
        }
        else if ((horiz = Input.GetAxis("Horizontal")) != 0 || (vert = Input.GetAxis("Vertical")) != 0)
        {
            return move;
        }
        else
        {
            return stand;
        }

    }
}