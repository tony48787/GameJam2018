﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelManager {

	public int playerLevel;
	public int vitality;		// guards HP
	public int skill;			// guards charge time, attack load speed
	public int strength;		// guards attack damage

	private GameManager gm;

	private PowerRule powerRuleForLevelUpCost;

	private PowerRule powerRuleForMaxHp;
	private PowerRule powerRuleForMaxChargeBarValue;
	private PowerRule powerRuleForChargeCoolDownDuration;
	private PowerRule powerRuleForChargeBarIncreaseRate;
	private PowerRule powerRuleForChargeBarDecreaseRate;
	private PowerRule powerRuleForShootCoolDownDuration;
	private PowerRule powerRuleForBulletDamage;
	private PowerRule powerRuleForBulletSpeed;
	private PowerRule powerRuleForSwordDamage;
	private PowerRule powerRuleForSwordRepelForce;
	private PowerRule powerRuleForChargeBulletDamage;
    private PowerRule powerRuleForChargeBulletSpeed;
    private PowerRule powerRuleForChargeSwordDamage;
    private PowerRule powerRuleForChargeSwordSpeed;


	// Use this for initialization
	public PlayerLevelManager () {
		gm = GameManager.instance;
		playerLevel = 1;
		vitality = 1;
		skill = 1;
		strength = 1;

		powerRuleForLevelUpCost = new PowerRule(300, 100, 800000);

		powerRuleForMaxHp = new PowerRule(100, 100, 2000);
		powerRuleForMaxChargeBarValue = new PowerRule(20, 1000, 1600);
		powerRuleForChargeCoolDownDuration = new PowerRule(20, 3000, 1000);		// div 1000
		powerRuleForChargeBarIncreaseRate = new PowerRule(20, 5000, 15000);		// div 1000
		powerRuleForChargeBarDecreaseRate = new PowerRule(20, 10000, 12000);	// div 1000
		powerRuleForShootCoolDownDuration = new PowerRule(20, 200, 80);			// div 1000
		powerRuleForBulletDamage = new PowerRule(100, 10, 1000);
		powerRuleForBulletSpeed = new PowerRule(20, 1500, 3000);				// div 100
		powerRuleForSwordDamage = new PowerRule(100, 10, 1200);
		powerRuleForSwordRepelForce = new PowerRule(20, 10, 30);
		powerRuleForChargeBulletDamage = new PowerRule(100, 100, 8000);
		powerRuleForChargeBulletSpeed = new PowerRule(20, 1000, 2000);			// div 100
		powerRuleForChargeSwordDamage = new PowerRule(100, 100, 6500);
		powerRuleForChargeSwordSpeed = new PowerRule(20, 300, 1000);			// div 100
	}

	public void LevelUpVitalityBy(int level) {
		SetVitalityToLevel(vitality + level);
	}

	public void LevelUpSkillBy(int level) {
		SetSkillToLevel(skill + level);
	}

	public void LevelUpStrengthBy(int level) {
		SetStrengthToLevel(strength + level);
	}

	public long GetCoinToLevelUpToLevel(int level) {
		return powerRuleForLevelUpCost.retrieveValueForLevel(level);
	}

	public void SetVitalityToLevel(int level) {
		playerLevel += level - vitality;
		vitality = level;
		gm.playerStatus.maxHp = (float) powerRuleForMaxHp.retrieveValueForLevel(level);
		gm.playerStatus.currentHp = gm.playerStatus.maxHp;
	}

	public void SetSkillToLevel(int level) {
		playerLevel += level - skill;
		skill = level;
		gm.playerStatus.maxChargeBarValue = (float) powerRuleForMaxChargeBarValue.retrieveValueForLevel(level);
        gm.playerStatus.chargeCoolDownDuration = (float) powerRuleForChargeCoolDownDuration.retrieveValueForLevel(level) / 1000;
        gm.playerStatus.chargeBarIncreaseRate = (float) powerRuleForChargeBarIncreaseRate.retrieveValueForLevel(level) / 1000;
        gm.playerStatus.chargeBarDecreaseRate = (float) powerRuleForChargeBarDecreaseRate.retrieveValueForLevel(level) / 1000;
		gm.playerStatus.shootCoolDownDuration = (float) powerRuleForShootCoolDownDuration.retrieveValueForLevel(level) / 1000;
	}

	public void SetStrengthToLevel(int level) {
		playerLevel += level - strength;
		strength = level;
		gm.weaponStatus.bulletDamage = (float) powerRuleForBulletDamage.retrieveValueForLevel(level);
        gm.weaponStatus.bulletSpeed = (float) powerRuleForBulletSpeed.retrieveValueForLevel(level) / 100;
        gm.weaponStatus.swordDamage = (float) powerRuleForSwordDamage.retrieveValueForLevel(level);
        gm.weaponStatus.swordRepelForce = (float) powerRuleForSwordRepelForce.retrieveValueForLevel(level);
        gm.weaponStatus.chargeBulletDamage = (float) powerRuleForChargeBulletDamage.retrieveValueForLevel(level);
        gm.weaponStatus.chargeBulletSpeed = (float) powerRuleForChargeBulletSpeed.retrieveValueForLevel(level) / 100;
        gm.weaponStatus.chargeSwordDamage = (float) powerRuleForChargeSwordDamage.retrieveValueForLevel(level);
        gm.weaponStatus.chargeSwordSpeed = (float) powerRuleForChargeSwordSpeed.retrieveValueForLevel(level) / 100;
	}
}
