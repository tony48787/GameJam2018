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
                GameObject tower = InitTower(spawnTransform);

                return tower;
            }
            else
            {
                Debug.Log("Not enough money");
            }
        } else
        {
            Debug.Log("No tower is selected");
        }
        return null;
    }

    public GameObject InitTower(Transform spawnTransform)
    {
        GameObject tower = Instantiate(selectedTowerPrefab, spawnTransform.position, Quaternion.identity);
        tower.transform.Translate(new Vector3(0.3f, -0.25f));

        GameManager.instance.IncrementCoinBy(-tower.GetComponentInChildren<TowerType>().buildCost);
        return tower;
    }

    public void SetTower(GameObject towerPrefab)
    {
        selectedTowerPrefab = towerPrefab;
    }

    public long GetTowerBuildCost()
    {
        return FetchTowerBuildCost();
    }

    public bool CheckCanBuildTower()
    {
        long buildCost = FetchTowerBuildCost();
        if (GameManager.instance.coin >= buildCost)
        {
            return true;
        }
        return false;
    }

    private long FetchTowerBuildCost()
    {
        return selectedTowerPrefab.GetComponentInChildren<TowerType>().buildCost;
    }
}
