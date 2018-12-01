using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour {

    [SerializeField]
    private long spawnCount = 1;

    [SerializeField]
    private long towerBuilderSpawnCount = 1;

    private long currentCount;

    private float vertExtent;

    private float horzExtent;

    private PowerRule powerRuleForEnemyNumbers;
    private PowerRule powerRuleForEnemyBuilderNumbers;
    private PowerRule powerRuleForEnemyMaxHp;
    private PowerRule powerRuleForEnemyCoinDrop;
    private PowerRule powerRuleForEnemyForce;
    private PowerRule powerRuleForEnemySpeed;

    private PowerRule powerRuleForMiniBossMaxHp;
    private PowerRule powerRuleForMiniBossCoinDrop;
    private PowerRule powerRuleForMiniBossForce;
    private PowerRule powerRuleForMiniBossSpeed;

    private bool isMiniBossSpawned;

    private EnemyTarget tileTarget;

    // Use this for initialization
    void Start () {
        vertExtent = GameManager.instance.vertExtent;
        horzExtent = GameManager.instance.horzExtent;
        powerRuleForEnemyNumbers = new PowerRule(30, 1, 100);
        powerRuleForEnemyBuilderNumbers = new PowerRule(30, 1, 15);
        powerRuleForEnemyMaxHp = new PowerRule(20, 20, 2000);
        powerRuleForEnemyCoinDrop = new PowerRule(20, 5, 30);
        powerRuleForEnemyForce = new PowerRule(20, 200, 600);
        powerRuleForEnemySpeed = new PowerRule(50, 1, 7);

        powerRuleForMiniBossMaxHp = new PowerRule(20, 100, 200000);
        powerRuleForMiniBossCoinDrop = new PowerRule(20, 20, 1000);
        powerRuleForMiniBossForce = new PowerRule(20, 400, 1200);
        powerRuleForMiniBossSpeed = new PowerRule(50, 2, 14);
    }

    void StartSpawn()
    {
        spawnCount = powerRuleForEnemyNumbers.retrieveValueForLevel(GameManager.instance.wave);
        towerBuilderSpawnCount = powerRuleForEnemyBuilderNumbers.retrieveValueForLevel(GameManager.instance.wave);
        currentCount = spawnCount + towerBuilderSpawnCount;

        isMiniBossSpawned = false;

        MarkTileAsTarget();

        EnemyTarget playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<EnemyTarget>();
        for (int i = 0; i < spawnCount; i++)
        {
            CreateSimpleEnemy(PrefabManager.instance.enemy, playerTarget);
        }

        for (int i = 0; i < towerBuilderSpawnCount; i++)
        {
            CreateSimpleEnemy(PrefabManager.instance.enemy_tower_builder, tileTarget);
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

    private GameObject CreateSimpleEnemy(GameObject enemyPrefab, EnemyTarget enemyTarget)
    {
        GameObject newEnemy = Instantiate(enemyPrefab);

        newEnemy.GetComponent<EnemyController>().Target = enemyTarget;

        float randX = Random.Range(-horzExtent * 0.8f, +horzExtent * 0.8f);
        float randY = Random.Range(vertExtent, vertExtent + vertExtent / 10);
        newEnemy.transform.position = new Vector3(randX, randY, 0);

        newEnemy.GetComponent<SimpleEnemyController>().healthBarType = PrefabManager.instance.healthBarType.GetComponent<HealthBar>();
        newEnemy.GetComponent<SimpleEnemyController>().maxHp = powerRuleForEnemyMaxHp.retrieveValueForLevel(GameManager.instance.wave);
        newEnemy.GetComponent<SimpleEnemyController>().coinDrop = (int) powerRuleForEnemyCoinDrop.retrieveValueForLevel(GameManager.instance.wave);

        newEnemy.GetComponent<EnemyController>().baseForce = powerRuleForEnemyForce.retrieveValueForLevel(GameManager.instance.wave);
        newEnemy.GetComponent<EnemyController>().Speed = powerRuleForEnemySpeed.retrieveValueForLevel(GameManager.instance.wave);
        return newEnemy;
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
        EnemyTarget playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<EnemyTarget>();
        GameObject newEnemy = CreateSimpleEnemy(PrefabManager.instance.enemy_tower_builder, playerTarget);
        newEnemy.transform.localScale = new Vector3(2, 2, 1);
        newEnemy.GetComponent<SimpleEnemyController>().maxHp = powerRuleForMiniBossMaxHp.retrieveValueForLevel(GameManager.instance.wave);
        newEnemy.GetComponent<SimpleEnemyController>().coinDrop = (int)powerRuleForMiniBossCoinDrop.retrieveValueForLevel(GameManager.instance.wave);

        newEnemy.GetComponent<EnemyController>().baseCoolDownTime = newEnemy.GetComponent<EnemyController>().baseCoolDownTime / 2f;
        newEnemy.GetComponent<EnemyController>().baseForce = powerRuleForMiniBossForce.retrieveValueForLevel(GameManager.instance.wave);
        newEnemy.GetComponent<EnemyController>().Speed = powerRuleForMiniBossSpeed.retrieveValueForLevel(GameManager.instance.wave);
    }
}
