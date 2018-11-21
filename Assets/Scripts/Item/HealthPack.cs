using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {

	public float recoverHp = 20f;
	private CircleCollider2D circleCollider2D;
	// Use this for initialization
	void Start () {
		circleCollider2D = GetComponent<CircleCollider2D>();
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (!other.isTrigger && other.CompareTag("Player")) {
			HeroController player = other.GetComponent<HeroController>();
			player.hp += recoverHp;
			if (player.hp >= player.maxHp) {
				player.hp = player.maxHp;
			}
			Destroy(gameObject);
		}
	}
}
