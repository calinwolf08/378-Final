using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

   // TODO: Generalize script

   public GameObject enemy;

   public int enemiesPerBlock;

	// Use this for initialization
	void Start () {
      System.Random rng = new System.Random();
        int numBlock1 = enemiesPerBlock;//rng.Next(1, enemiesPerBlock);
      int numBlock2 = rng.Next(1, enemiesPerBlock);
      int numBlock3 = rng.Next(1, enemiesPerBlock);
      int numBlock4 = rng.Next(1, enemiesPerBlock);

      float posBlock1 = -2;

	   // Spawn block 1
      for (int i = 0; i < numBlock1; i++) {
         Instantiate(enemy, new Vector3(0, (float)3.8, posBlock1), Quaternion.identity);
         posBlock1 -= (float)1.5;
      }
	}
}
