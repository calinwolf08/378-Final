using UnityEngine;
using System.Collections;

public class Recolor : MonoBehaviour {

   public float r;
   public float g;
   public float b;

	// Use this for initialization
	void Start () {
      GetComponent<Renderer>().material.color = new Color(r, g, b);
	}
	
	// Update is called once per frame
	void Update () {
	  
	}
}
