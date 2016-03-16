using UnityEngine;
using System.Collections;

public class SurfaceTexture : MonoBehaviour {

	public Renderer rend;
    public float x, y;

    void Start() {
        rend = GetComponent<Renderer>();

        float scaleX = x;
        float scaleY = y;
        rend.material.mainTextureScale = new Vector2(scaleX, scaleY);
    }
    void Update() {
        
    }
}
