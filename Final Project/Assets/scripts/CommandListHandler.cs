using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CommandListHandler : MonoBehaviour {

   private bool paused = false;

	// Use this for initialization
	void Start () {
      GetComponent<Canvas>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	   if (Input.GetKeyDown(KeyCode.Escape)) {
         if (paused) {
            Time.timeScale = 1;
            paused = false;
            GetComponent<Canvas>().enabled = false;
         }
         else {
            Time.timeScale = 0;
            paused = true;
            GetComponent<Canvas>().enabled = true;
         }
      }
	}
}
