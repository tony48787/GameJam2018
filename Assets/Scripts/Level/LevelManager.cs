using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    private string[] tileMaps;

    private GameObject startWaveBtn;
    private GameObject levelUpBtn;
    private GameObject levelUpMenu;

    // Use this for initialization
    void Start ()
    {
        startWaveBtn = GameObject.Find("StartWaveBtn").gameObject;
        levelUpBtn = GameObject.Find("LevelUpBtn").gameObject;
        levelUpMenu = GameObject.Find("PlayerMenu").gameObject;
        levelUpMenu.SetActive(false);
        CreateLevel();
    }

    private void CreateLevel()
    {
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        GameObject tilePrefab = PrefabManager.instance.tile;
        float tileSize = tilePrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        int rowMax = (int) Math.Floor(Math.Abs(worldStart.y) / tileSize * 2);
        int colMax = (int) Math.Floor(Math.Abs(worldStart.x) / tileSize * 2);

        tileMaps = CreateTileMaps(rowMax, colMax);

        for (int row = 0; row < rowMax; row++)
        {
            for (int col = 0; col < colMax; col++)
            {
                PlaceTile(row, col, worldStart);
            }
        }

    }

    private void PlaceTile(int row, int col, Vector3 worldStart)
    {
        GameObject tilePrefab = PrefabManager.instance.tile;
        if (int.Parse(tileMaps[row][col].ToString()) == 1)
        {
            float tileSize = tilePrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

            GameObject newTile = Instantiate(tilePrefab);

            newTile.transform.position = new Vector3(worldStart.x + (tileSize * col), worldStart.y - (tileSize * row), 0);

            newTile.GetComponent<TileScript>().Init(new Point(col, row), 1);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void StartWave()
    {
        gameObject.SendMessage("StartSpawn");

        startWaveBtn.SetActive(false);
        levelUpBtn.SetActive(false);

        GameManager.instance.gameState = GameState.Playing;

        foreach (TowerController tower in FindObjectsOfType<TowerController>())
        {
            tower.isActive = false;
        }

    }

    public void EndWave()
    {
        startWaveBtn.SetActive(true);
        levelUpBtn.SetActive(true);

        GameManager.instance.IncrementWaveBy();
        GameManager.instance.gameState = GameState.Transiting;

        foreach (TowerController tower in FindObjectsOfType<TowerController>())
        {
            tower.isActive = false;
        }
    }

    public void DevpKillEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            if (enemy) {
                enemy.GetComponent<EnemyController>().OnDespawn();
            }
        }
    }

    private string[] ReadTextFiles()
    {
        TextAsset textAsset = (TextAsset) Resources.Load("level");
        return textAsset.text.Split(new[] { Environment.NewLine },
            StringSplitOptions.None);
    }

    private string[] CreateTileMaps(int rowMax, int colMax)
    {
        string[] tileMaps = new string[rowMax];
        for (int row = 0; row < rowMax; row++)
        {
            string rowStr = "";
            for (int col = 0; col < colMax; col++)
            {
                if (row == rowMax - 1 || col == 0 || col == colMax - 1)
                {
                    rowStr += "1";
                } else
                {
                    rowStr += "0";
                }
            }
            tileMaps[row] = rowStr;
        }
        return tileMaps;
    }

    public void OpenLevelUpMenu()
    {
        levelUpBtn.SetActive(false);
        levelUpMenu.SetActive(true);
    }

    public void CloseLevelUpMenu()
    {
        levelUpMenu.SetActive(false);
        levelUpBtn.SetActive(true);
    }

}
