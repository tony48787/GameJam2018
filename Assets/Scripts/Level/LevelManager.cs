﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    private string[] tileMaps;

    private GameObject startWaveBtn;

    // Use this for initialization
    void Start ()
    {
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

        startWaveBtn = GameObject.Find("StartWaveBtn").gameObject;
        startWaveBtn.SetActive(false);

        foreach (TowerController tc in FindObjectsOfType<TowerController>()) {
            tc.isActive = true;
        }
    }

    public void EndWave()
    {
        startWaveBtn.SetActive(true);

        GameManager.instance.IncrementWaveBy();

        foreach (TowerController tc in FindObjectsOfType<TowerController>())
        {
            tc.isActive = false;
        }
    }

    public void DevpKillEnemy()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Enemy");
        if (go)
        {
            go.GetComponent<EnemyController>().OnDespawn();
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
}
