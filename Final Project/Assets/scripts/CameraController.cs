using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;
    //private float offset;
	// Use this for initialization
	void Start () {
        //offset = transform.position.y - player.transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(player.transform.position.x,
            transform.position.y, transform.position.z);
	}
}
