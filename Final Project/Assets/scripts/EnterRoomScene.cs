using UnityEngine;
using System.Collections;

public class EnterRoomScene : MonoBehaviour {

    PlayerController playerScript;
    BetaEnemySpawner enemySpawner;
    Animator animator;
    Rigidbody rigidBody;
    GameObject player;

    private bool start, exit;
    public float speed, playerEndLoc;

	void Start () {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
        enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<BetaEnemySpawner>();
        playerScript.enabled = false;
        enemySpawner.enabled = false;

        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        start = true;
        exit = false;

        StartCoroutine(pause());
    }

    void beginToExit() {
        exit = true;
        animator.SetBool("Walk", true);

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    IEnumerator pause() {
        yield return new WaitForSeconds(2);
    }

	void Update () {

	    if (start) {
            animator.SetTrigger("Summon");
            player.GetComponent<Animator>().SetBool("Running", true);
            
            start = false;
        }

        if (exit) {
            rigidBody.velocity = Vector3.right * speed;
        }

        if (player.transform.position.x < playerEndLoc) {
            player.GetComponent<Rigidbody>().velocity = Vector3.right * player.GetComponent<PlayerController>().speed;
        } else {
            player.GetComponent<Animator>().SetBool("Running", false);
        }
	}

    //move until out of camera
    void OnBecameInvisible() {
        playerScript.enabled = true;
        enemySpawner.enabled = true;
        Destroy(this.gameObject);
    }
}
