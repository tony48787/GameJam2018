using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    private Point pointInGrid;

    private int tileType;

    private bool isOccupied;

    private GameObject tower;

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
        if (Input.GetMouseButtonDown(0)) {
            if(!isOccupied)
            {
                tower = FindObjectOfType<TowerSpawner>().SpawnTower(transform);
                if (tower) {
                    isOccupied = true;
                }
            } else
            {
                //Todo
                Debug.Log("Show UI");
            }
        } 
    }
}
