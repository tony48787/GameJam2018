using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {

    public GameObject towerBullet01;

    public GameObject healthBarType;

    public GameObject tile;

    public GameObject tower;

    public GameObject enemy;

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
