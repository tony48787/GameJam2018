using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField]
    private long spawnCount = 1;

    private long currentCount;

    private float vertExtent;

    private float horzExtent;

    private PowerRule powerRule;

    // Use this for initialization
    void Start () {
        vertExtent = GameManager.instance.vertExtent;
        horzExtent = GameManager.instance.horzExtent;
        powerRule = new PowerRule(20, 1, 200);
    }

    void StartSpawn()
    {
        spawnCount = powerRule.retrieveValueForLevel(GameManager.instance.wave);
        currentCount = spawnCount;

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject newEnemy = Instantiate(PrefabManager.instance.enemy);

            newEnemy.GetComponent<EnemyController>().Target = FindObjectOfType<EnemyTarget>();

            float randX = Random.Range(-horzExtent, +horzExtent);
            float randY = Random.Range(vertExtent, vertExtent + vertExtent / 10);
            newEnemy.transform.position = new Vector3(randX, randY, 0);

            newEnemy.GetComponent<SimpleEnemyController>().healthBarType = PrefabManager.instance.healthBarType.GetComponent<HealthBar>();
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
