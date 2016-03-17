using UnityEngine;
using System.Collections;

public class punisher : MonoBehaviour {
    private Animator anim;
    private Rigidbody rig;
    private ParticleSystem par;
    private BoxCollider collider;
    private SphereCollider sphereCollider;
    private CapsuleCollider capsule;
    private SpriteRenderer render;
    public Transform punish;

    public float speed;
    public float zSpeed;
    private float moveSpeed;

    private Vector3 toMove;
    private Vector3 foundYou;
    private bool dying;
    private bool pound = false;
    private bool hammerLoc = false;

    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        par = GetComponent<ParticleSystem>();
        collider = GetComponent<BoxCollider>();
        sphereCollider = GetComponent<SphereCollider>();
        render = GetComponent<SpriteRenderer>();

        foundYou = Vector3.zero;
        moveSpeed = speed;
        dying = false;
    }

    // Update is called once per frame
    void Update() {
        if (anim.GetBool("death")) {
            StartCoroutine(waitDeath());
        }
        else if (!foundYou.Equals(Vector3.zero) && !dying) {
            transform.position += trackLocation(foundYou, transform.position);
        }
        else if (!dying) {
            setFacing(speed);
            transform.position += new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
        }
    }

    void OnCollisionEnter(Collision col) {

        if (col.gameObject.CompareTag("Fireball")) {
            anim.SetBool("death", true);
        }

        if (col.gameObject.CompareTag("Wall")) {
            speed *= -1;
        }
    }

    void OnTriggerEnter(Collider other) {
        bool skipLR = other.gameObject.CompareTag("Wall");
        bool skipBase = other.gameObject.CompareTag("Ground");// || other.gameObject.CompareTag("Back Wall");

        if (other.gameObject.CompareTag("Player") && foundYou.Equals(Vector3.zero) && !hammerLoc) {
            StartCoroutine(waitOnNextLocation(other));
        }

        if (!skipLR && !skipBase && anim.GetBool("hammerTime") && !pound) {
            StartCoroutine(waitGroundPound(other));
        }
    }

    void OnTriggerExit(Collider other) {
        bool skipLR = other.gameObject.CompareTag("Wall");
        bool skipBase = other.gameObject.CompareTag("Ground");// || other.gameObject.CompareTag("Back Wall");

        if (other.gameObject.CompareTag("Player") && foundYou.Equals(Vector3.zero) && !hammerLoc) {
            StartCoroutine(waitOnNextLocation(other));
        }

        if (!skipLR && !skipBase && anim.GetBool("hammerTime") && !pound) {
            StartCoroutine(waitGroundPound(other));
        }

        if (other.name == "Ground") {
            speed *= -1;
        }
    }

    void OnTriggerStay(Collider other) {
        bool skipLR = other.gameObject.CompareTag("Wall");
        bool skipBase = other.gameObject.CompareTag("Ground");// || other.gameObject.CompareTag("Back Wall");

        if (other.gameObject.CompareTag("Player") && foundYou.Equals(Vector3.zero) && !hammerLoc) {
            StartCoroutine(waitOnNextLocation(other));
        }

        if (!skipLR && !skipBase && anim.GetBool("hammerTime") && !pound) {
            StartCoroutine(waitGroundPound(other));
        }
    }

    Vector3 trackLocation(Vector3 player, Vector3 currLocation) {
        float z = 0f, x = 0f;

        if (Mathf.Abs(player.x - currLocation.x) <= 1 &&
            Mathf.Abs(player.z - currLocation.z) <= 1) {
            StartCoroutine(nextHammerTime());
            return Vector3.zero;
        }

        if (player.x > currLocation.x)
            x = Mathf.Abs(moveSpeed);
        else if (player.x < currLocation.x)
            x = moveSpeed;

        setFacing(x);
        if (player.z > currLocation.z)
            z = Mathf.Abs(zSpeed);
        else if (player.z < currLocation.z)
            z = zSpeed;

        return new Vector3(x * Time.deltaTime, 0f, z);
    }

    IEnumerator waitDeath() {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }

    IEnumerator nextHammerTime() {
        anim.SetBool("hammerTime", true);
        rig.isKinematic = true;
        yield return new WaitForSeconds(2f);
        rig.isKinematic = false;
        anim.SetBool("hammerTime", false);
        foundYou = Vector3.zero;
    }

    IEnumerator waitGroundPound(Collider other) {
        pound = true;
        yield return new WaitForSeconds(.5f);
        other.transform.position += new Vector3(0f, 2f, 0f);
        pound = false;
    }

    IEnumerator waitOnNextLocation(Collider other) {
        hammerLoc = true;
        yield return new WaitForSeconds(2);
        foundYou = other.transform.position;
        hammerLoc = false;
    }

    public void setFacing(float xdir) {
        if (xdir < 0)
            render.flipX = false;
        else
            render.flipX = true;
    }
}
