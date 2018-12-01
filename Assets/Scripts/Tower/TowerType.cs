using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerType : MonoBehaviour {
    public long buildCost = 100;
    public float baseDamage = 10.0f;
    public GameObject towerPrefab;
    public PowerRule powerRuleForUpgradeCost;
    public PowerRule powerRuleForDamage;

    void Start () {
        powerRuleForUpgradeCost = new PowerRule(50, buildCost, buildCost * buildCost);
        long damage = (long) baseDamage;
        powerRuleForDamage = new PowerRule(50, 10, 900);
    }
}
