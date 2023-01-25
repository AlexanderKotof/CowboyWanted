using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointComponent : MonoBehaviour
{
    [Serializable]
    public class EnemiesWave
    {
        public EnemyComponent[] enemies;

        public int enemiesCount;

        public float spawnRate;
        public float maxEnemiesCount;
    }

    public EnemiesWave enemiesWave;
}
