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

    public bool isActive = true;

    public TowerOwner owner;

    public int level = 1;

    public float damage = 50.0f;

    public void UpgradeTowerByCoin()
    {
        long upgradeCost = towerType.powerRuleForUpgradeCost.retrieveValueForLevel(level);
        if (GameManager.instance.coin >= upgradeCost)
        {
            if (owner == TowerOwner.HERO)
            {
                UpgradeTower();

                GameManager.instance.IncrementCoinBy(-upgradeCost);
            } else
            {
                Debug.Log("This is not your tower anymore.");
            }
            
        }

    }

    public void UpgradeTower()
    {
        level += 1;
        damage = towerType.powerRuleForDamage.retrieveValueForLevel(level);
        towerLevelText.text = level.ToString();
    }

    public long GetUpgradeCost()
    {
        return towerType.powerRuleForUpgradeCost.retrieveValueForLevel(level);
    }

    public void UpdateOwnerToEnemy()
    {
        owner = TowerOwner.ENEMY;
        enemy = GameObject.FindGameObjectWithTag("Player");
        GetComponent<SpriteRenderer>().color = new Color(248f / 255f, 109f / 255f, 255f / 255f);
    }

    // Use this for initialization
    void Start () {
		towerType = GetComponent<TowerType>();
        towerLevelText = transform.parent.GetComponentInChildren<TextMeshProUGUI>();
        bulletTransform = GetComponentInChildren<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        if(isActive)
        {
            if (!overheat)
            {

                if (owner == TowerOwner.ENEMY)
                {
                    ShootEnemy();
                }
                else if (owner == TowerOwner.HERO)
                {
                    if (EnemyInRange())
                    {
                        ShootEnemy();
                    }
                    else
                    {
                        enemy = FindClosestEnemy();
                    }
                }
            }
            else
            {
                Cooldown();
            }
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

        if(owner == TowerOwner.ENEMY)
        {
            gm.GetComponentInChildren<BulletController>().owner = BulletOwner.ENEMY;
        }
    }

    void Cooldown()
    {
        if ((Time.time - lastShootTime) > 1.0f)
        {
            overheat = false;
        }
    }
}

public enum TowerOwner
{
    ENEMY,
    HERO
}