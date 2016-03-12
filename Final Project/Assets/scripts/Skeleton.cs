using UnityEngine;
using System.Collections;

public class Skeleton : MonoBehaviour {

    private Animator anim;
    private Rigidbody rig;
    private ParticleSystem par;
    private new BoxCollider collider;
    private SphereCollider sphereCollider;
    private SpriteRenderer render;
    public Transform skel, explosion;

    public Vector3 alignSizeDeath;
    public Vector3 alignCenterDeath;

    public Vector3 alignSizeWalking;
    public Vector3 alignCenterWalking;

    private Vector3 blowPostion;
    private Vector3 triggerPosition;

    private Vector3 toMove;
    private Vector3 forceDirection;

    private GameObject enemySpawner;

    public float speed;
    public float accel;
    public float hitForce, explodeForce;
    public float zfollow;
    public int health;

    private float moveSpeed;
    private bool blow;
    private bool exploding;
    private bool newSpawn = false; //false if not already respawning
    private bool inForce;
    private bool searching = true;

    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        par = GetComponent<ParticleSystem>();
        collider = GetComponent<BoxCollider>();
        sphereCollider = GetComponent<SphereCollider>();
        render = GetComponent<SpriteRenderer>();
        enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner");

        collider.size = alignSizeWalking;
        collider.center = alignCenterWalking;
        moveSpeed = speed;
        forceDirection = Vector3.right;
    }

    // Update is called once per frame
    void Update() {
        setFacing(speed);
        bool respawn = health > 0 && anim.GetBool("death"); //if blown up and can respawn
        bool headingTowardPlayer = anim.GetBool("collision"); //if heading toward player to blow up
        bool walking = !respawn && !headingTowardPlayer && !anim.GetBool("death"); //if pacing

        if (health <= 0 && !anim.GetBool("death")) {
            anim.SetBool("death", true);
            StartCoroutine(waitSpawn());
            collider.center = alignCenterDeath;
            collider.size = alignSizeDeath;
        } else if (walking) {
            transform.position += new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
        }
        else if (headingTowardPlayer && !exploding) { //move toward player until exploding
            transform.position += getBlowLocation(blowPostion, transform.position);
        }
        else if (respawn && !newSpawn) { //either respawns or completely dies
            StartCoroutine(waitSpawn());
        }

    }

    bool isGrounded(Collision col) {
        Vector3 norm = col.contacts[0].normal;

        if (norm.y > 0) {
            return true;
        }

        return false;
    }

    void OnCollisionEnter(Collision col) {
        //if hit player or fireball and not exploding lose health
        if (((col.gameObject.CompareTag("Player") && col.gameObject.GetComponent<PlayerController>().executing)
            || col.gameObject.CompareTag("Fireball")) && !exploding) {
            anim.SetBool("collision", false);
            health--;
        }

        //if hit wall turn around
        if (col.gameObject.CompareTag("Wall")) {
            speed *= -1;
        }

        //if you collide with the ground after exploding
        if (col.gameObject.CompareTag("Ground") && exploding) {
            anim.SetBool("death", true);
            anim.SetBool("collision", false);
            collider.center = alignCenterDeath;
            collider.size = alignSizeDeath;
            exploding = false;
        }

        if (col.gameObject.CompareTag("Ground") && anim.GetBool("death")) {
            rig.isKinematic = true;
            collider.enabled = false;
        }
    }

    void OnTriggerStay(Collider other) {
        //if player enters sphere and not already found
        if (other.gameObject.CompareTag("Player") && !anim.GetBool("collision") && !anim.GetBool("death")) {
            anim.SetBool("collision", true); //starts sequence for walking toward player and exploding
            blowPostion = other.transform.position; //position to walk toward
        }
    } 
    
    Vector3 getBlowLocation(Vector3 player, Vector3 currLocation) {
        float z = 0f, x = 0f;

        //if already at location
        if (Mathf.Abs(player.x - currLocation.x) <= 1 &&
            Mathf.Abs(player.z - currLocation.z) <= 1) {

            waitExplosion();
            return Vector3.zero;
        } if (player.x > currLocation.x) {
            x = Mathf.Abs(moveSpeed);
        }
        else if (player.x < currLocation.x) {
            x = moveSpeed;
        }

        setFacing(x);
        if (player.z > currLocation.z)
            z = Mathf.Abs(zfollow);
        else if (player.z < currLocation.z)
            z = zfollow;

        return new Vector3(x * Time.deltaTime, 0f, z);
    }

    //waits 2 seconds to spawn
    IEnumerator waitSpawn() {
        newSpawn = true; //boolean to keep method from getting called again in update

        yield return new WaitForSeconds(2); //wait until respawn or disappear from map

        if (health > 0) { //if can still respawn
            rig.isKinematic = false;
            collider.enabled = true;
            newSpawn = false; //set so can respawn again next time
            collider.center = alignCenterWalking;
            collider.size = alignSizeWalking;
            Instantiate(skel, transform.position, transform.rotation);
        }
        else {
            enemySpawner.GetComponent<BetaEnemySpawner>().decrementNumEnemies();
        }

        Destroy(this.gameObject);
    }

    void explode() { //explosion should touch skeleton
        Vector3 spawnPos = transform.position;
        Instantiate(explosion, spawnPos, transform.rotation);
    }

    void waitExplosion() {
        rig.AddForce(Vector3.up * explodeForce, ForceMode.VelocityChange);
        anim.SetTrigger("foundYou"); //start collision animation
        collider.center = alignCenterDeath;
        collider.size = alignSizeDeath;
        explode(); //start explosion
        exploding = true;
    }

    public void setFacing(float xdir) {
        if (xdir < 0) {
            render.flipX = false;
            forceDirection = Vector3.right;
        }
        else {
            render.flipX = true;
            forceDirection = Vector3.left;
        }
    }
}