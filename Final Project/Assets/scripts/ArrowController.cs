using UnityEngine;
using System.Collections;

public class ArrowController : MonoBehaviour {
    public PlayerController player;
    private int pos;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerController>();
        pos = player.getNumInputs() - 1;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = player.transform.position;

        Vector3 offset = transform.position;
        offset.x -= (float)2.5;
        offset.y += 5;
        offset.x += (pos % 4) * 2;
        offset.y -= (pos / 4) * (float)2.7;
        transform.position = offset;

        if (player.getNumInputs() == 0)
        {
            Destroy(gameObject);
        }
	}
}
