using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        for (int i = 0; i < spawnCount; i++)
        {
            CreateSimpleEnemy();
        }
    }

    private GameObject CreateSimpleEnemy()
    {
        GameObject newEnemy = Instantiate(PrefabManager.instance.enemy);

        newEnemy.GetComponent<EnemyController>().Target = FindObjectOfType<EnemyTarget>();

        float randX = Random.Range(-horzExtent, +horzExtent);
        float randY = Random.Range(vertExtent, vertExtent + vertExtent / 10);
        newEnemy.transform.position = new Vector3(randX, randY, 0);

        newEnemy.GetComponent<SimpleEnemyController>().healthBarType = PrefabManager.instance.healthBarType.GetComponent<HealthBar>();
        newEnemy.GetComponent<SimpleEnemyController>().maxHp = powerRuleForEnemyMaxHp.retrieveValueForLevel(GameManager.instance.wave);
        newEnemy.GetComponent<SimpleEnemyController>().coinDrop = (int) powerRuleForEnemyCoinDrop.retrieveValueForLevel(GameManager.instance.wave);
        return newEnemy;
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
