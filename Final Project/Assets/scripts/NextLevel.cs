using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NextLevel : MonoBehaviour {

   public GameObject enemySpawner;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.CompareTag("Player")) {
            if (enemySpawner.GetComponent<BetaEnemySpawner>().getEnemiesLeft() <= 0) {
                SceneManager.LoadScene("player");
            }
        }
    }
}
