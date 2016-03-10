using UnityEngine;
using System.Collections;

public class Skeleton : MonoBehaviour {

	private Animator anim;
	private Rigidbody rig;
	private ParticleSystem par;
	private BoxCollider collider;
	private SphereCollider sphereCollider;
	private SpriteRenderer render;
	public Transform skel;

    public Vector3 alignSizeDeath;
    public Vector3 alignCenterDeath;

    public Vector3 alignSizeWalking;
    public Vector3 alignCenterWalking;

    public GameObject enemySpawner;

	private Vector3 blowPostion;
	private Vector3 triggerPosition;

	private Vector3 toMove;

	public float speed;
	public float accel;
    public float hitForce;
	public float zfollow;
	public int health;

	private bool blow;
	private bool newSpawn = false;

    // temporary
    private bool dead = false;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rig = GetComponent<Rigidbody>();
		par = GetComponent<ParticleSystem> ();
		collider = GetComponent<BoxCollider> ();
		sphereCollider = GetComponent<SphereCollider> ();
		render = GetComponent<SpriteRenderer> ();

        collider.size = alignSizeWalking;
        collider.center = alignCenterWalking;
    }

	// Update is called once per frame
	void Update () {
		if (speed < 0)
			render.flipX = false;
		else
			render.flipX = true;

		if (!anim.GetBool ("death") && !anim.GetBool("collision") && !blow) {
			transform.position += new Vector3 (speed * Time.deltaTime, 0.0f, 0.0f);
			anim.SetFloat ("speed", speed);
		} else if (blow && !anim.GetBool("death")) {
			toMove = getBlowLocation (blowPostion, transform.position);
			transform.position += toMove;
		} else if (health > 0 && !anim.GetBool("collision") && anim.GetBool ("death") && !newSpawn) {
			newSpawn = true;
			StartCoroutine (waitSpawn ());
		}

        if (!dead && health <= 0) {
            dead = true;
            enemySpawner.GetComponent<BetaEnemySpawner>().setEnemiesLeft(enemySpawner.GetComponent<BetaEnemySpawner>().getEnemiesLeft() - 1);
        }
    }

    public bool isDead() {
        return anim.GetBool("death");
    }

	void OnCollisionEnter(Collision col){
		if (col.gameObject.CompareTag("Player") && !anim.GetBool ("death")) {
			setForce (transform.right);
			anim.SetBool ("death", true);
			health--;

            // Update enemies left
            enemySpawner.GetComponent<BetaEnemySpawner>().setEnemiesLeft(enemySpawner.GetComponent<BetaEnemySpawner>().getEnemiesLeft() - 1);
        }
    }

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Player") && !anim.GetBool("death") && !anim.GetBool("collision") && !blow) {
			//speed -= accel;
			blow = true;
			blowPostion = other.transform.position;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("Player") && !anim.GetBool("death")) {
			//speed += accel;
		}
		if (other.gameObject.CompareTag("Ground")) {
			speed *= -1;
		}
	}

	Vector3 getBlowLocation(Vector3 player, Vector3 currLocation){
		float z=0f, x=0f;

		if (Mathf.Abs(player.x - currLocation.x) <= 1 &&
			Mathf.Abs(player.z - currLocation.z) <= 1) {
			anim.SetBool ("collision", true);
			blow = false;
			StartCoroutine (waitExplosion ());
			return Vector3.zero;
		}
		if (player.x > currLocation.x) {
			x = Mathf.Abs (speed);
			render.flipX = true;
		} else if (player.x < currLocation.x) {
			x = speed;
			render.flipX = false;
		}

		if (player.z > currLocation.z) {
			z = Mathf.Abs (zfollow);
		} else if (player.z < currLocation.z) {
			z = zfollow;
		} 
		toMove = new Vector3 (x*Time.deltaTime,0f,z);
		
		return toMove;
	}

	void setForce(Vector3 direction){
		rig.AddForce (transform.TransformPoint(direction * hitForce));
		rig.useGravity = true;
		//collider.size = alignSizeDeath;
		//collider.center = alignCenterDeath;
	}

	IEnumerator waitSpawn() {
		yield return new WaitForSeconds(2);
		Instantiate (skel,transform.position,transform.rotation);
		Destroy (this.gameObject);
	}

	IEnumerator waitExplosion() {
		yield return new WaitForSeconds(2);
		anim.SetBool ("collision",false);
		anim.SetBool ("death", true);
		setForce (transform.right);
	}
}