using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour {

    private GameObject selectedTowerPrefab;

    void Start()
    {
        selectedTowerPrefab = PrefabManager.instance.tower;
    }

    public GameObject SpawnTower(Transform spawnTransform)
    {
        if (selectedTowerPrefab)
        {
            long buildCost = FetchTowerBuildCost();
            if (GameManager.instance.coin >= buildCost)
            {
                GameObject tower = Instantiate(selectedTowerPrefab, spawnTransform.position, Quaternion.identity);
                tower.transform.Translate(new Vector3(0.3f, -0.25f));

                GameManager.instance.IncrementCoinBy(-tower.GetComponent<TowerType>().buildCost);

                return tower;
            } else
            {
                Debug.Log("Not enough money");
            }
        } else
        {
            Debug.Log("No tower is selected");
        }
        return null;
    }

    public void SetTower(GameObject towerPrefab)
    {
        selectedTowerPrefab = towerPrefab;
    }

    private long FetchTowerBuildCost()
    {
        return selectedTowerPrefab.GetComponent<TowerType>().buildCost;
    }
}
