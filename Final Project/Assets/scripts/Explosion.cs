using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
    private ParticleSystem ps;
    public float duration, damage;

	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        if (ps.time >= duration) {
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.CompareTag("Player")) {
            col.gameObject.GetComponent<PlayerController>().getHit(transform.position, damage);
        }
    }
}
