using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {

    private bool overheat;

    private float lastShootTime;

    private GameObject enemy;

    private float damage = 50.0f;

    public int buildCost = 100;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (EnemyInRange() && !overheat) {
            ShootEnemy();
        } else
        {
            Cooldown();
        }

        
	}

    bool EnemyInRange()
    {
        if (enemy)
        {
            float dist = Vector3.Distance(gameObject.transform.position, enemy.transform.position);
            return dist < GameManager.instance.horzExtent / 2;
        } else
        {
            return false;
        }
        
    }

    void ShootEnemy()
    {
        overheat = true;
        lastShootTime = Time.time;

        Transform transform = GetComponent<Transform>();
        Vector3 diff = enemy.transform.position - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        Transform bulletTransform = GetComponentInChildren<Transform>();
        GameObject gm = Instantiate(PrefabManager.instance.towerBullet01, bulletTransform.position, bulletTransform.rotation);
        gm.GetComponentInChildren<BulletController>().damage = damage;
    }

    void Cooldown()
    {
        if ((Time.time - lastShootTime) > 1.0f)
        {
            overheat = false;
        }
    }
}
