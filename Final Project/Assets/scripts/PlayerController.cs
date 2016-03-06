using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private new BoxCollider collider;
    private int mode;
    private int stand = 0;
    private int move = 1;
    private int attack = 2;
    private int jumping = 3;
    private int attackOverride = 0;
    private int timer; 
    private string input = "";
    public float speed, lean, jumpForce, dashForce, timeScale, punchForce;
    public int direction, specialTime; // -1 for left, 1 for right
    public Vector3 standCenter;// = new Vector3((float)-.2, (float)-.4, 0);
    public Vector3 standSize;// = new Vector3((float)1.3, (float)4.3, (float).8);
    public Vector3 runCenter;// = new Vector3((float)-.45, (float)-.87, 0);
    public Vector3 runSize;// = new Vector3((float)2.2, (float)3.4, (float).8);
    public float horiz;
    public float vert;
    private bool isGrounded, goingIn, goingOut;
    public Transform fireball;

    //moves --> 0 is facing right, 1 is facing left
    private string[] shoot = {"dr", "dl"};
    private string[] dashF = {"rr", "ll", "uu"};
    private string[] dashB = { "ll", "rr", "dd"};
    private string[] punch = { "r", "l" };

    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
        collider = this.GetComponent<BoxCollider>();
        isGrounded = true;
        goingIn = goingIn = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * collider.bounds.extents.y, Color.green);
        mode = getKey();

        if (mode == attack || attackOverride == 1)
        {
            special();

            if (isPressingRunKeys() && !animator.GetBool("Jumping") 
                && !animator.GetBool("Dashing")) { 
                run();
            } else if (!animator.GetBool("Jumping") && !animator.GetBool("Dashing")) {
                idle();
            }
        }
        else if (mode == move && !animator.GetBool("Dashing"))
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

    void OnCollisionEnter(Collision col) {
        if ((col.gameObject.CompareTag("Ground")) && !isGrounded) {
            Vector3 norm = col.contacts[0].normal;

            if (norm.y > 0) {
                isGrounded = true;
                animator.enabled = true;
                animator.SetBool("Jumping", false);
                animator.SetBool("Dashing", false);
            }
        }
    }

    void jump() {
        //if havent started jump yet, start animation
        if (isGrounded) {
            animator.SetTrigger("Jump");
            animator.SetBool("Jumping", true);
        }

        if (isPressingRunKeys()) {
            run();
        }

    }

    //adds force to rigibbody to make player jump
    void takeAir() {
        //add velocity
        if (isGrounded) {
            rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
            isGrounded = false;
        }
    }

    //called by jump animation to keep top of jump frame activated until landing
    void stillInAir() {
        if (!isGrounded) {
            animator.enabled = false;
        }
    }

    //dash in a direction based on dir
    void dash(int dir) {

        Vector3 toDash = Vector3.up * .5f;

        if (goingIn) {
            toDash += Vector3.forward;
            goingIn = false;
        } else if (goingOut) {
            toDash += Vector3.back;
            goingOut = false;
        } else {
            if (direction < 0) {    //facing left
                if (dir < 0) {      //dash back
                    toDash += Vector3.right;
                }
                else {            //dash forward
                    toDash += Vector3.left;
                }
            }
            else {                //facing right
                if (dir < 0) {      //dash back
                    toDash += Vector3.left;
                }
                else {            //dash forward
                    toDash += Vector3.right;
                }
            }
        }
        
        animator.SetBool("Dashing", true);
        rb.AddForce(toDash * dashForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void completePunch() {
        if (direction < 0) {
            rb.AddForce(Vector3.left * punchForce, ForceMode.Impulse);
        } else {
            rb.AddForce(Vector3.right * punchForce, ForceMode.Impulse);
        }
    }

    void shootFireball() {
        Vector3 spawnPos = transform.position;
        spawnPos.x += direction;
        Instantiate(fireball, spawnPos, transform.rotation);
    }

    void idle()
    {
        if (animator.GetBool("Running"))
        { //if running
            animator.SetBool("Running", false);

            BoxCollider bc = GetComponent<BoxCollider>();
            bc.center = standCenter;
            bc.size = standSize;
        } //else if (animator.GetBool("Dashing")) {
          //  animator.SetBool("Jumping", false);
       // }

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
        if (!animator.GetBool("Running"))
        {   //if not already running
            animator.SetBool("Running", true);

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
            ret = Vector3.left;
        }
        else {
            ret = Vector3.right;
        }

        return ret;
    }

    //for moving player in and out
    Vector3 moveInOut(float vert)
    {
        if (!animator.GetBool("Running"))
        {   //if not already running
            animator.SetBool("Running", true);

            BoxCollider bc = GetComponent<BoxCollider>();
            bc.center = runCenter;
            bc.size = runSize;
        }

        Vector3 ret;

        //move in or out and rotate
        if (vert < 0)
        {
            this.transform.localEulerAngles = new Vector3((float)-lean, (float)0, (float)0);
            ret = Vector3.back;
        }
        else {
            this.transform.localEulerAngles = new Vector3((float)lean, (float)0, (float)0);
            ret = Vector3.forward;
        }

        return ret;
    }

    void run()
    {
        Vector3 toMove = new Vector3(0,0,0);//transform.position;

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
        
        Debug.DrawRay(transform.position, toMove, Color.green);

        if (animator.GetBool("Jumping")) {
            rb.AddForce(toMove * speed * 2, ForceMode.Force);
        } else {
            rb.velocity = toMove * speed;
        }
    }

    void executeMove(string input) {
        bool validInput = false;

        if ((direction > 0 && input == shoot[0]) || (direction < 0 && input == shoot[1])) {
            validInput = true;
            animator.SetTrigger("Shoot");
            animator.enabled = true;
        } else if ((direction > 0 && input == dashF[0]) || (direction < 0 && input == dashF[1]) ||
            input == dashF[2]) {
            validInput = true;

            if (input == dashF[2]) {
                goingIn = true;
            }

            animator.SetTrigger("DashForward");
            animator.SetBool("Dashing", true);
        } else if ((direction > 0 && input == dashB[0]) || (direction < 0 && input == dashB[1]) ||
            input == dashB[2]) {
            validInput = true;

            if (input == dashB[2]) {
                goingOut = true;
            }

            animator.SetTrigger("DashBack");
            animator.SetBool("Dashing", true);
        } else if ((direction > 0 && input == punch[0]) || (direction < 0 && input == punch[1])) {
            validInput = true;
            animator.SetTrigger("Punch");
            animator.enabled = true;
        }

        if (validInput) {
            animator.enabled = true;
        }
    }

    void special()
    {
        if (!animator.GetBool("SpecialAttack"))
        {
            animator.SetBool("SpecialAttack", true);
            Time.timeScale = timeScale;
            timer = specialTime;
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

        if (Input.GetKeyUp(KeyCode.Space) || timer <= 0)
        {
            attackOverride = 0;
            animator.SetBool("SpecialAttack", false);
            executeMove(input);
            input = "";
            Time.timeScale = 1;
        }

        timer--;
    }

    int getKey()
    {
        //if starting special attack or already entering attack
        if (Input.GetKeyDown(KeyCode.Space) || attackOverride == 1)
        {
            attackOverride = 1;
            return attack;
        }

        //if starting jump or already jumping
        if (Input.GetKeyDown(KeyCode.CapsLock) || !isGrounded) {
            return jumping;
        }

        //if trying to run
        if (isPressingRunKeys() && !animator.GetBool("Jumping") 
            && !animator.GetBool("Dashing"))
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