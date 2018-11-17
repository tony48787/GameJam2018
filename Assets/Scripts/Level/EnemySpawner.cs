using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private int spawnCount = 10;

    private int currentCount;

	// Use this for initialization
	void Start () {
		
	}

    void StartSpawn()
    {
        currentCount = spawnCount;

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject newEnemy = Instantiate(enemy);
        }
    }

    void EndSpawn()
    {
        gameObject.SendMessage("EndWave");
    }

    public void UpdateCurrentCountBy(int delta = 1)
    {
        currentCount -= delta;
        if (currentCount <= 0)
        {
            EndSpawn();
        }
    }
}
