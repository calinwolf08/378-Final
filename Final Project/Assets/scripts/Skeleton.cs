using UnityEngine;
using System.Collections;

public class Skeleton : MonoBehaviour {

	private Animator anim;
	private Rigidbody rig;
	private ParticleSystem par;
	private BoxCollider collider;

    public Vector3 alignSizeDeath;
    public Vector3 alignCenterDeath;

    public Vector3 alignSizeWalking;
    public Vector3 alignCenterWalking;

    public float speed;
	public float accel;
    public float hitForce;

    private Vector3 toMove;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rig = GetComponent<Rigidbody>();
		par = GetComponent<ParticleSystem> ();
		collider = GetComponent<BoxCollider> ();

        collider.size = alignSizeWalking;
        collider.center = alignCenterWalking;
    }

	// Update is called once per frame
	void Update () {

		if (!anim.GetBool("death"))
			transform.position += new Vector3 (speed * Time.deltaTime, 0.0f, 0.0f);
	}

	void OnMouseDown(){
		rig.AddForce (transform.TransformPoint(transform.right * 600));
		rig.useGravity = true;
        anim.SetBool("death", true);
        anim.Play ("skel_collision");
		//Destroy (gameObject,3);

	}

	void OnCollisionEnter(Collision col){
		if(col.gameObject.name == "Player" && !anim.GetBool("death")) {
			rig.AddForce (transform.TransformPoint(transform.right * hitForce));
			rig.useGravity = true;
            anim.SetBool("death", true);
            collider.size = alignSizeDeath;
            collider.center = alignCenterDeath;
            anim.Play ("skel_collision");
		} else if (col.gameObject.name == "Player") {
            rig.isKinematic = true;
            rig.detectCollisions = false;
        }
	}

}
