﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private int spawnCount = 10;

    private int currentCount;

    private float vertExtent;

    private float horzExtent;

    // Use this for initialization
    void Start () {
        vertExtent = Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
    }

    void StartSpawn()
    {
        currentCount = spawnCount;

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject newEnemy = Instantiate(enemy);

            newEnemy.GetComponent<EnemyController>().Target = FindObjectOfType<EnemyTarget>();

            float randX = Random.Range(-horzExtent, +horzExtent);
            float randY = Random.Range(vertExtent, vertExtent + vertExtent / 10);
            newEnemy.transform.position = new Vector3(randX, randY, 0);
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
