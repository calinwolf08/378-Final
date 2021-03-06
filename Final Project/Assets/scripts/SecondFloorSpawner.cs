﻿using UnityEngine;
using System.Collections;

public class SecondFloorSpawner : MonoBehaviour {

   public GameObject skeleton;
   public GameObject punisher;

   public int enemiesPerBlock;

   public int numEnemies;

   // Enemies per each zone
   private int enemiesBlock1;
   private int enemiesBlock2;
   private int enemiesBlock3;

   // Use this for initialization
   void Start() {
      System.Random rng = new System.Random();

      enemiesBlock1 = rng.Next(1, enemiesPerBlock);
      enemiesBlock2 = rng.Next(1, enemiesPerBlock);
      enemiesBlock3 = rng.Next(1, enemiesPerBlock);

      numEnemies = enemiesBlock1 + enemiesBlock2 + enemiesBlock3;

      // Spawn enemies in zone 1
      for (int i = 0; i < enemiesBlock1; i++) {
         // Instantiate(randomEnemy(rng), new Vector3(22, (float)2.55, 3 + i), Quaternion.identity);
         Instantiate(skeleton, new Vector3((float)rng.Next(14, 17), (float)2.55, (float)rng.Next(-11, 3)), Quaternion.identity);
      }

      // Spawn enemies in zone 2
      for (int i = 0; i < enemiesBlock2; i++) {
         // Instantiate(randomEnemy(rng), new Vector3(42, (float)2.55, 3 + i), Quaternion.identity);
         Instantiate(skeleton, new Vector3((float)rng.Next(45, 49), (float)2.55, (float)rng.Next(-11, 3)), Quaternion.identity);
      }

      // Spawn enemies in zone 3
      for (int i = 0; i < enemiesBlock3; i++) {
         // Instantiate(randomEnemy(rng), new Vector3(62, (float)2.55, 3 + i), Quaternion.identity);
         Instantiate(skeleton, new Vector3((float)rng.Next(59, 65), (float)2.55, (float)rng.Next(-11, 3)), Quaternion.identity);
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
}
