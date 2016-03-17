using UnityEngine;
using System.Collections;

public class NewLevel : MonoBehaviour {


	// Use this for initialization
	void Start () {
      GameObject camera = GameObject.Find("Main Camera");
      GameObject player = GameObject.FindGameObjectWithTag("Player");

      camera.GetComponent<CameraController>().player = player;
      //player.transform.position = new Vector3((float)-15.85, (float)2.6, (float)-6.59);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
