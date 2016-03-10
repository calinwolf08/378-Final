using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NextLevel : MonoBehaviour {

   private GameObject enemySpawner;

	// Use this for initialization
	void Start () {
        enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner");
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
