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
    private GameObject restartBtn;
    private GameObject levelUpMenu;

    // Use this for initialization
    void Start ()
    {
        startWaveBtn = GameObject.Find("StartWaveBtn").gameObject;
        levelUpBtn = GameObject.Find("LevelUpBtn").gameObject;
        restartBtn = GameObject.Find("RestartBtn").gameObject;
        levelUpMenu = GameObject.Find("PlayerMenu").gameObject;
        levelUpMenu.SetActive(false);
        restartBtn.GetComponent<Button>().onClick.AddListener(() => OnRestartBtnClicked());
        restartBtn.SetActive(false);
        CreateLevel();
        CreateHelpIcon();
        SetBtnIsActive(false);
    }

    private void CreateHelpIcon()
    {
        Vector3 newPosition = new Vector3(-GameManager.instance.horzExtent / 4, 0, 0);
        Instantiate(PrefabManager.instance.helpIcon, newPosition, Quaternion.identity).GetComponent<HelpIcon>().current = 0;
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

        SetBtnIsActive(false);

        GameManager.instance.gameState = GameState.Playing;

        foreach (TowerController tower in FindObjectsOfType<TowerController>())
        {
            tower.isActive = false;
        }

    }

    public void EndWave()
    {
        SetBtnIsActive(true);

        GameManager.instance.IncrementWaveBy();
        GameManager.instance.gameState = GameState.Transiting;

        foreach (TowerController tower in FindObjectsOfType<TowerController>())
        {
            tower.isActive = false;
        }
    }

    public void DevpKillEnemy()
    {
        GetComponent<EnemySpawner>().EndSpawn();
        Destroy(GetComponent<EnemySpawner>());

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            if (enemy) {
                Destroy(enemy);
            }
        }

        GameObject[] healthBars = GameObject.FindGameObjectsWithTag("HealthBar");
        foreach (GameObject healthBar in healthBars) {
            if (healthBar) {
                Destroy(healthBar);
            }
        }

        gameObject.AddComponent<EnemySpawner>();
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

    public void SetBtnIsActive(bool isActive)
    {
        startWaveBtn.SetActive(isActive);
        levelUpBtn.SetActive(isActive);
    }

    public void EnableRestartBtn()
    {
        restartBtn.SetActive(true);
    }

    public void OnRestartBtnClicked()
    {
        restartBtn.SetActive(false);
        SetBtnIsActive(true);
        GameManager.instance.RestartGame();
    }
}
