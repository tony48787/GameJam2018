﻿using System.Collections;
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

	// Use this for initialization
	void Start () {
        gm = GameManager.instance;
        towerSpawner = FindObjectOfType<TowerSpawner>();
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
        if (Input.GetMouseButtonDown(0)) {
            if(!isOccupied)
            {
                tower = towerSpawner.SpawnTower(transform);
                if (tower) {
                    towerController = tower.GetComponentInChildren<TowerController>();
                    towerType = tower.GetComponentInChildren<TowerType>();
                    isOccupied = true;
                    gm.ShowHintText("Coin needed: " + towerType.buildCost);
                }
            } else
            {
                //Todo
                Debug.Log("Show UI");
                towerController.UpgradeTower();
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
