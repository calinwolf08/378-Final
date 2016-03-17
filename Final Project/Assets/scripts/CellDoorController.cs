using UnityEngine;
using System.Collections;

public class CellDoorController : MonoBehaviour {

    private GameObject es;
    public bool enemiesGone; //can make private once working
    public float openAngle, rotateSpeed;

	// Use this for initialization
	void Start () {
        es = GameObject.FindGameObjectWithTag("EnemySpawner");
	}
	
	// Update is called once per frame
	void Update () {
        if (enemiesGone && gameObject.transform.eulerAngles.y >= openAngle) {
            transform.Rotate(Vector3.up * Time.deltaTime * -1 * rotateSpeed, Space.World);
        }
	}

    void OnTriggerStay(Collider other) {
        //set trigger to open door when enemies are gone and player gets close
        if (other.gameObject.CompareTag("Player") &&
            es.gameObject.GetComponent<BetaEnemySpawner>().numEnemies == 0) {
            enemiesGone = true;
        }
    }
}
