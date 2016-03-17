using UnityEngine;
using System.Collections;

public class EntranceDoorController : MonoBehaviour {
    
    public float openAngle, rotateSpeed;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (gameObject.transform.eulerAngles.y <= openAngle) {
            transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed, Space.World);
        }
    }
}
