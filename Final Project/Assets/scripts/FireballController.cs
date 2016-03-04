using UnityEngine;
using System.Collections;

public class FireballController : MonoBehaviour {

    public float speed;
    public float maxDistance;
    public PlayerController player;

    private float initialPos;
    private float currentPos;
    private float distanceTraveled = 0;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>();
        initialPos = GetComponent<Transform>().position.x;
        currentPos = GetComponent<Transform>().position.x;

        if (player.transform.localScale.x < 0)
            speed = -speed;
        GetComponent<Rigidbody>().velocity = new Vector3(speed, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        currentPos = GetComponent<Transform>().position.x;

        if (currentPos - initialPos >= maxDistance || currentPos - initialPos <= -maxDistance) {
            Destroy(gameObject);
        }
	}
}
