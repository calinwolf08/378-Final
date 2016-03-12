using UnityEngine;
using System.Collections;

public class BetaEnemySpawner : MonoBehaviour {

   public GameObject skeleton;
   public GameObject punisher;

   public int enemiesPerBlock;

   public int numEnemies;

   // Enemies per each zone
   private int enemiesBlock1;
   private int enemiesBlock2;
   private int enemiesBlock3;

	// Use this for initialization
	void Start () {
      System.Random rng = new System.Random();

        enemiesBlock1 = enemiesPerBlock;//rng.Next(1, enemiesPerBlock);
        enemiesBlock2 = enemiesPerBlock;//rng.Next(1, enemiesPerBlock);
        enemiesBlock3 = enemiesPerBlock;//rng.Next(1, enemiesPerBlock);

      numEnemies = enemiesBlock1 + enemiesBlock2 + enemiesBlock3;

      // Spawn enemies in zone 1
      for (int i = 0; i < enemiesBlock1; i++) {
         // Instantiate(randomEnemy(rng), new Vector3(22, (float)2.55, 3 + i), Quaternion.identity);
         Instantiate(skeleton, new Vector3(22, (float)2.55, -2 * i), Quaternion.identity);
      }

      // Spawn enemies in zone 2
      for (int i = 0; i < enemiesBlock2; i++) {
         // Instantiate(randomEnemy(rng), new Vector3(42, (float)2.55, 3 + i), Quaternion.identity);
         Instantiate(skeleton, new Vector3(42, (float)2.55, -2 * i), Quaternion.identity);
      }

      // Spawn enemies in zone 3
      for (int i = 0; i < enemiesBlock3; i++) {
         // Instantiate(randomEnemy(rng), new Vector3(62, (float)2.55, 3 + i), Quaternion.identity);
         Instantiate(skeleton, new Vector3(62, (float)2.55, -2 * i), Quaternion.identity);
      }
	}

   private GameObject randomEnemy(System.Random rng) {
      int randomEnemy = rng.Next(1, 3);

      switch (randomEnemy) {
         case 1:
            return skeleton;
            break;
         case 2:
            return punisher;
            break;
         default:
            return punisher;
            break;
      }
   }

   public int getEnemiesLeft() {
      return numEnemies;
   }

    public void setEnemiesLeft(int newNumEnemies) {
        numEnemies = newNumEnemies;
    }

    public void decrementNumEnemies() {
        numEnemies--;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
