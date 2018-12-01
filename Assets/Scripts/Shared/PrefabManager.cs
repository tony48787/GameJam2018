using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {

    public GameObject towerBullet01;

    public GameObject healthBarType;

    public GameObject chargeBarType;

    public GameObject progressBarType;

    public GameObject tile;

    public GameObject tower;

    public GameObject enemy;

    public GameObject enemy_tower_builder;

    public GameObject player;

    public GameObject bulletType;

    public GameObject explosiveBulletType;

    public GameObject swordWindType;

    public GameObject healthPackType;

    public Texture2D crosshairCursorType;

    public Texture2D addTowerCursorType;

    public Texture2D upgradeTowerCursorType;

    public Texture2D pickerCursorType;

    public GameObject helpIcon;

    public static PrefabManager instance = null;

    //Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
