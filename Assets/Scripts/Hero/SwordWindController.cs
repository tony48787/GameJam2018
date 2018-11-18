﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWindController : MonoBehaviour {

	public float damage = 100f;
	public float speed = 3f;
	public float maxLifeTime = 5f;
	private float activeTime = 0f;
	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		rb2d.velocity = rb2d.transform.up * speed;
	}

	// Update is called once per frame
	void Update () {
		activeTime += Time.deltaTime;
		if (activeTime > maxLifeTime) {
			Destroy(this.gameObject);
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (!other.isTrigger && other.CompareTag("Enemy")) {
			Debug.Log("Sword wind hits enemy!");
			DamageMessage msg = new DamageMessage(damage);
			other.SendMessageUpwards("OnDamaged", msg, SendMessageOptions.DontRequireReceiver);
		}
	}
}