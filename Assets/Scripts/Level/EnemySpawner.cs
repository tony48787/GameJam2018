using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour {

    [SerializeField]
    private long spawnCount = 1;

    private long currentCount;

    private float vertExtent;

    private float horzExtent;

    private PowerRule powerRuleForEnemyNumbers;
    private PowerRule powerRuleForEnemyMaxHp;
    private PowerRule powerRuleForEnemyCoinDrop;

    private PowerRule powerRuleForMiniBossMaxHp;
    private PowerRule powerRuleForMiniBossCoinDrop;

    private bool isMiniBossSpawned;

    private EnemyTarget tileTarget;

    // Use this for initialization
    void Start () {
        vertExtent = GameManager.instance.vertExtent;
        horzExtent = GameManager.instance.horzExtent;
        powerRuleForEnemyNumbers = new PowerRule(20, 1, 200);
        powerRuleForEnemyMaxHp = new PowerRule(20, 20, 2000);
        powerRuleForEnemyCoinDrop = new PowerRule(20, 5, 30);

        powerRuleForMiniBossMaxHp = new PowerRule(20, 100, 200000);
        powerRuleForMiniBossCoinDrop = new PowerRule(20, 20, 1000);
    }

    void StartSpawn()
    {
        spawnCount = powerRuleForEnemyNumbers.retrieveValueForLevel(GameManager.instance.wave);
        currentCount = spawnCount;
        isMiniBossSpawned = false;

        MarkTileAsTarget();

        for (int i = 0; i < spawnCount; i++)
        {
            CreateSimpleEnemy();
        }
    }

    private void MarkTileAsTarget()
    {
        TileScript[] tiles = FindObjectsOfType<TileScript>();
        int index = (int) Random.Range(0, tiles.Length);

        GameObject tile = tiles[index].gameObject;
        tile.AddComponent<EnemyTarget>();
        tile.GetComponent<SpriteRenderer>().color = new Color(222f / 255f, 71f / 255f, 224f /255f);
        tileTarget = tile.GetComponent<EnemyTarget>();
    }

    private GameObject CreateSimpleEnemy()
    {
        GameObject newEnemy = Instantiate(PrefabManager.instance.enemy);

        newEnemy.GetComponent<EnemyController>().Target = PickEnemyTarget();

        float randX = Random.Range(-horzExtent * 0.8f, +horzExtent * 0.8f);
        float randY = Random.Range(vertExtent, vertExtent + vertExtent / 10);
        newEnemy.transform.position = new Vector3(randX, randY, 0);

        newEnemy.GetComponent<SimpleEnemyController>().healthBarType = PrefabManager.instance.healthBarType.GetComponent<HealthBar>();
        newEnemy.GetComponent<SimpleEnemyController>().maxHp = powerRuleForEnemyMaxHp.retrieveValueForLevel(GameManager.instance.wave);
        newEnemy.GetComponent<SimpleEnemyController>().coinDrop = (int) powerRuleForEnemyCoinDrop.retrieveValueForLevel(GameManager.instance.wave);
        return newEnemy;
    }

    private EnemyTarget PickEnemyTarget()
    {
        float max = 10;
        float odds =  Random.Range(0, max);
        Debug.Log(odds);
        if (odds > max * 0.9)
        {
            Debug.Log("HI");
            return GameObject.FindGameObjectWithTag("Player").GetComponent<EnemyTarget>();
        } else
        {
            return tileTarget;
        }
    }

    void EndSpawn()
    {
        gameObject.SendMessage("EndWave");

        UnmarkTileAsTarget();
    }

    private void UnmarkTileAsTarget()
    {
        tileTarget.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        Destroy(tileTarget);
    }

    public void UpdateCurrentCountBy(int delta = 1)
    {
        currentCount -= delta;
        if (currentCount <= 0)
        {
            if (isMiniBossSpawned)
            {
                EndSpawn();
            } else
            {
                SpawnMiniBoss();
                isMiniBossSpawned = true;
            }
            
        }
    }

    void SpawnMiniBoss()
    {
        GameObject newEnemy = CreateSimpleEnemy();
        newEnemy.transform.localScale = new Vector3(2, 2, 1);
        newEnemy.GetComponent<SimpleEnemyController>().maxHp = powerRuleForMiniBossMaxHp.retrieveValueForLevel(GameManager.instance.wave);
        newEnemy.GetComponent<SimpleEnemyController>().coinDrop = (int)powerRuleForMiniBossCoinDrop.retrieveValueForLevel(GameManager.instance.wave);
    }
}
