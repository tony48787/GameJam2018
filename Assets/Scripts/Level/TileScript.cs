﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    private Point pointInGrid;

    private int tileType;

    private bool isOccupied;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(Point p, int tileType)
    {
        this.pointInGrid = p;
        this.tileType = tileType;
        this.isOccupied = false;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !isOccupied )
        {
            isOccupied = true;

            GameObject tower = Instantiate(PrefabManager.instance.tower, transform.position, Quaternion.identity);

            tower.transform.Translate(new Vector3(0.3f, -0.25f));

            GameManager.instance.IncrementCoinBy(-tower.GetComponent<TowerController>().buildCost);
        }
    }
}
