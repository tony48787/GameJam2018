using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelManagerScript : MonoBehaviour {

    [SerializeField]
    private GameObject tile;

    [SerializeField]
    private GameObject tower;

    private string[] tileMaps;

    private GameObject startWaveBtn;

    // Use this for initialization
    void Start ()
    {
        tileMaps = ReadTextFiles();
        CreateLevel();
    }

    private void CreateLevel()
    {
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        int rowMax = tileMaps.Length;
        int colMax = tileMaps[0].Length;
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
        if (int.Parse(tileMaps[row][col].ToString()) == 1)
        {
            float tileSize = tile.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

            GameObject newTile = Instantiate(tile);

            newTile.transform.position = new Vector3(worldStart.x + (tileSize * col), worldStart.y - (tileSize * row), 0);

            newTile.GetComponent<TileScript>().Init(new Point(col, row), 1);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    public GameObject getTower()
    {
        return tower;
    }

    public void StartWave()
    {
        gameObject.SendMessage("StartSpawn");

        startWaveBtn = GameObject.Find("StartWaveBtn").gameObject;
        startWaveBtn.SetActive(false);
    }

    public void EndWave()
    {
        startWaveBtn.SetActive(true);

        GameManager.instance.IncrementWaveBy();
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
}
