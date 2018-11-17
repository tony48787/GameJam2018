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