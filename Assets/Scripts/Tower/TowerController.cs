using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {

    private bool overheat;

    private float lastShootTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//if in range
            //if no cooldown then shoot
        if (EnemyInRange() && !overheat) {
            ShootEnemy();
        } else
        {
            Cooldown();
        }

        
	}

    bool EnemyInRange()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy)
        {
            float dist = Vector3.Distance(gameObject.transform.position, enemy.transform.position);
            return dist < GameManager.instance.horzExtent / 10;
        } else
        {
            return false;
        }
        
    }

    void ShootEnemy()
    {
        overheat = false;
        lastShootTime = Time.time;
        Transform transform = GetComponentInChildren<Transform>();
        GameObject gm = Instantiate(TowerManager.instance.bullet01, transform.position, transform.rotation);

    }

    void Cooldown()
    {
        if ((Time.time - lastShootTime) > 2.0f)
        {
            overheat = false;
        }
    }
}
