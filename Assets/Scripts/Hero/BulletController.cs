﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public BulletOwner owner = BulletOwner.HERO;

    private GameManager gm;
	private WeaponStatus status;

	public float speed = 10f;
	public float damage = 10f;
	private Rigidbody2D bulletBody;
	private float maxLifeTime = 3f;
	private float activeTime = 0f;
    
	// Use this for initialization
	void Start () {
		gm = GameManager.instance;
		status = gm.weaponStatus;

		bulletBody = GetComponent<Rigidbody2D>();
		bulletBody.velocity = bulletBody.transform.up * status.bulletSpeed;

        GetComponent<AudioSource>().Play();
    }
	
	// Update is called once per frame
	void Update () {
		activeTime += Time.deltaTime;
		if (activeTime > maxLifeTime) {
			Destroy(transform.parent.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.CompareTag("Enemy") && owner == BulletOwner.HERO) {
			Debug.Log("Bullet collide with Enemy");
			DamageMessage msg = new DamageMessage(status.bulletDamage);
			other.collider.SendMessageUpwards("OnDamaged", msg, SendMessageOptions.DontRequireReceiver);
		} else if (other.collider.CompareTag("Player") && owner == BulletOwner.ENEMY)
        {
            Debug.Log("Bullet collide with Enemy");
            DamageMessage msg = new DamageMessage(status.bulletDamage);
            other.collider.SendMessageUpwards("OnDamaged", msg, SendMessageOptions.DontRequireReceiver);
        }
        Destroy(transform.parent.gameObject);
	}
}

public enum BulletOwner
{
    HERO,
    ENEMY
}