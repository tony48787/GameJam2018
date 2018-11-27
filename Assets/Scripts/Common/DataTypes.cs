using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAttackType {
	Shoot, Slice
};

public struct DamageMessage {
	public float damage;
	public GameObject damageCauser;
	public Vector2 repelForce;

	public DamageMessage(float damage) {
		this.damage = damage;
		this.damageCauser = null;
		this.repelForce = new Vector2(0, 0);
	}

	public DamageMessage(float damage, GameObject damageCauser) {
		this.damage = damage;
		this.damageCauser = damageCauser;
		this.repelForce = new Vector2(0, 0);
	}

	public DamageMessage(float damage, Vector2 damageForce) {
		this.damage = damage;
		this.damageCauser = null;
		this.repelForce = damageForce;
	} 

	public DamageMessage(float damage, GameObject damageCauser, Vector2 damageForce) {
		this.damage = damage;
		this.damageCauser = damageCauser;
		this.repelForce = damageForce;
	} 
};

public struct PlayerStatus {
	public float maxHp;
	public float currentHp;
	public float maxChargeBarValue;
	public float currentChargeBarValue;
	public float chargeCoolDownDuration;
	public float chargeBarIncreaseRate;
	public float chargeBarDecreaseRate;
	public float shootCoolDownDuration;
	public float invincibleDuration;
	public PlayerAttackType currentAttackType;
};

public struct WeaponStatus {
	public float bulletDamage;
	public float bulletSpeed;
	public float swordDamage;
	public float swordRepelForce;
	public float chargeBulletDamage;
	public float chargeBulletSpeed;
	public float chargeSwordDamage;
	public float chargeSwordSpeed;
}

public enum MouseInputStatus {
	Attack, AddTower, UpgradeTower, InteractUI
}