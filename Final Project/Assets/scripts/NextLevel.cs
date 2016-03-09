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
      // Check this when colliding with player
      enemySpawner.GetComponent<BetaEnemySpawner>().getEnemiesLeft();
      // Call this when colliding with player and getEnemiesLeft() returns 0
      // SceneManager.LoadScene("player");
	}
}
