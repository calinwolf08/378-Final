using UnityEngine;
using System.Collections;

public class punisher : MonoBehaviour {
	private Animator anim;
	private Rigidbody rig;
	private ParticleSystem par;
	private BoxCollider collider;
	private SphereCollider sphereCollider;
	private SpriteRenderer render;
	public Transform punish; 

	public float speed;
	public float zSpeed;

	private Vector3 toMove;
	private Vector3 foundYou;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rig = GetComponent<Rigidbody>();
		par = GetComponent<ParticleSystem> ();
		collider = GetComponent<BoxCollider> ();
		sphereCollider = GetComponent<SphereCollider> ();
		render = GetComponent<SpriteRenderer> ();

		foundYou = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		if (!foundYou.Equals (Vector3.zero)) {
			transform.position += trackLocation (foundYou, transform.position);
		} else
			transform.position += new Vector3 (speed * Time.deltaTime, 0.0f, 0.0f);
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.name == "Player") {
			
		} else if (col.gameObject.name == "Player") {
			//rig.isKinematic = true;
			//rig.detectCollisions = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.name=="Player" && foundYou.Equals(Vector3.zero)) {
			foundYou = other.transform.position;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.name == "Player" ) {
			
		}
		if (other.name == "Ground") {
			speed *= -1;
			if (speed < 0)
				render.flipX = false;
			else
				render.flipX = true;
		}
	}

	Vector3 trackLocation(Vector3 player, Vector3 currLocation){
		float z=0f, x=0f;

		if (Mathf.Abs(player.x - currLocation.x) <= 1 &&
			Mathf.Abs(player.z - currLocation.z) <= 1) {
			anim.SetBool ("hammerTime",true);
			StartCoroutine (waitHammerTime());
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
			z = Mathf.Abs (zSpeed);
		} else if (player.z < currLocation.z) {
			z = zSpeed;
		} 
		toMove = new Vector3 (x*Time.deltaTime,0f,z);

		return toMove;
	}

	IEnumerator waitHammerTime() {
		yield return new WaitForSeconds(1);
		anim.SetBool ("hammerTime",false);
		foundYou = Vector3.zero;
		render.flipX = false;
		sphereCollider.isTrigger = true;
		//Instantiate (punish,transform.position,transform.rotation);
		//Destroy (this.gameObject);
	}
}
