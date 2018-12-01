using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    private Point pointInGrid;

    private int tileType;

    private bool isOccupied;

    private GameObject tower;

    private GameManager gm;

    private TowerSpawner towerSpawner;
    
    private TowerController towerController;

    private TowerType towerType;

    private TowerOwner owner;

    private float enemyTowerProgress;

    private HealthBar progressBar;

	// Use this for initialization
	void Start () {
        gm = GameManager.instance;
        towerSpawner = FindObjectOfType<TowerSpawner>();
	}
	
	void FixedUpdate() {
        EnemyTarget enemyTarget = GetComponent<EnemyTarget>();
        if(enemyTarget)
        {
            //Attach progress bar
            if(!progressBar)
            {
                GameObject go = Instantiate(PrefabManager.instance.progressBarType, transform.position, Quaternion.identity);
                go.transform.Translate(new Vector3(0.3f, +0.5f));
                progressBar = go.GetComponent<HealthBar>();
            }

            //Detect if any enemy in range
            GameObject[] enemies = EnemiesInRange();

            if (enemies.Length > 0)
            {
                enemyTowerProgress += enemies.Length / 5f;
                progressBar.SetHealthRatio(enemyTowerProgress / 100f);
                if (enemyTowerProgress >= 100f)
                {
                    if (towerController == null)
                    {
                        tower = FindObjectOfType<TowerSpawner>().InitTower(transform);
                        towerController = tower.GetComponentInChildren<TowerController>();
                        towerType = tower.GetComponentInChildren<TowerType>();
                        isOccupied = true;
                        towerController.UpdateOwnerToEnemy();
                    } else if (towerController.owner != TowerOwner.ENEMY)
                    {
                        towerController.UpdateOwnerToEnemy();
                    } else  if (towerController.owner == TowerOwner.ENEMY)
                    {
                        towerController.UpgradeTower();
                    }

                    Destroy(progressBar.gameObject);
                    progressBar = null;
                    enemyTowerProgress = 0f;
                }
            }
        } else
        {
            enemyTowerProgress = 0f;
            if (progressBar)
            {
                Destroy(progressBar.gameObject);
                progressBar = null;
            }
        }
	}

    GameObject[] EnemiesInRange()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");

        List<GameObject> result = new List<GameObject>();
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            if(EnemyInRange(go))
            {
                result.Add(go);
            }
        }
        return result.ToArray();
    }

    bool EnemyInRange(GameObject enemy)
    {
        if (enemy)
        {
            float dist = Vector3.Distance(gameObject.transform.position, enemy.transform.position);
            return dist < GameManager.instance.horzExtent / 4;
        }
        else
        {
            return false;
        }

    }

    public void Init(Point p, int tileType)
    {
        this.pointInGrid = p;
        this.tileType = tileType;
        this.isOccupied = false;
    }

    private void OnMouseOver()
    {
        if (!isOccupied) {
            gm.mouseInputStatus = MouseInputState.AddTower;
            // TODO: get build cost
            gm.ShowHintText("Coin needed: " + towerSpawner.GetTowerBuildCost());
        }
        else {
            gm.mouseInputStatus = MouseInputState.UpgradeTower;
            // TODO: get upgrade cost
            gm.ShowHintText("Coin needed: " + towerController.GetUpgradeCost());
        }
        gm.UpdateCursorTexture();
        if (Input.GetMouseButtonDown(0) && gm.gameState != GameState.Tutorial) {
            if(!isOccupied)
            {
                tower = towerSpawner.SpawnTower(transform);
                if (tower) {
                    towerController = tower.GetComponentInChildren<TowerController>();
                    towerType = tower.GetComponentInChildren<TowerType>();
                    isOccupied = true;
                    gm.ShowHintText("Coin needed: " + towerType.buildCost);

                    towerController.owner = TowerOwner.HERO;
                }
            } else
            {
                //Todo
                Debug.Log("Show UI");
                towerController.UpgradeTowerByCoin();
                gm.ShowHintText("Coin needed: " + towerType.buildCost);
            }
        } 
    }

    private void OnMouseExit()
    {
        gm.mouseInputStatus = MouseInputState.Attack;
        gm.HideHintText();
        gm.UpdateCursorTexture();
    }
}

