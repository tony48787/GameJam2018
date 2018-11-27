using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerController : MonoBehaviour {

    private bool overheat;

    private float lastShootTime;

    private GameObject enemy;

    private TowerType towerType;

    private TextMeshProUGUI towerLevelText;

    private Transform bulletTransform;

    public int level = 1;

    public float damage = 50.0f;

    public void UpgradeTower()
    {
        long upgradeCost = towerType.powerRuleForUpgradeCost.retrieveValueForLevel(level);
        if (GameManager.instance.coin >= upgradeCost)
        {
            level += 1;
            damage = towerType.powerRuleForDamage.retrieveValueForLevel(level);

            GameManager.instance.IncrementCoinBy(-upgradeCost);
            towerLevelText.text = level.ToString();
        }
        
    }

    public long GetUpgradeCost()
    {
        return towerType.powerRuleForUpgradeCost.retrieveValueForLevel(level);
    }

    // Use this for initialization
    void Start () {
		towerType = GetComponent<TowerType>();
        towerLevelText = transform.parent.GetComponentInChildren<TextMeshProUGUI>();
        bulletTransform = GetComponentInChildren<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!overheat) {
            if (EnemyInRange())
            {
                ShootEnemy();
            } else
            {
                enemy = FindClosestEnemy();
            }
        } else
        {
            Cooldown();
        }

        
	}

    GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
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

        Vector3 diff = enemy.transform.position - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

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
