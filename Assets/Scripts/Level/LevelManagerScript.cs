using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerScript : MonoBehaviour {

    [SerializeField]
    private GameObject tile;

    [SerializeField]
    private GameObject tower;

    // Use this for initialization
    void Start ()
    {
        CreateLevel();
    }

    private void CreateLevel()
    {
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        int rowMax = 15;
        int colMax = 38;
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
        float tileSize = tile.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        GameObject newTile = Instantiate(tile);

        newTile.transform.position = new Vector3(worldStart.x + (tileSize * col), worldStart.y - (tileSize * row), 0);

        newTile.GetComponent<TileScript>().Init(new Point(col, row), 1);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public GameObject getTower()
    {
        return tower;
    }
}
