using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour {

	private GameManager gm;
	
	public float damage = 10f;
	public float repelForce = 10f;

	// Use this for initialization
	void Start () {
		gm = GameManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log("Trigger eneter");
		if (!other.isTrigger && other.CompareTag("Enemy")) {
			// add damage and repel enemy from this collider
			Rigidbody2D otherRb2d = other.GetComponent<Rigidbody2D>();
			Vector2 direction = otherRb2d.position - new Vector2 (transform.position.x, transform.position.y);
			direction.Normalize();
			DamageMessage msg = new DamageMessage(gm.weaponStatus.swordDamage, direction * gm.weaponStatus.swordRepelForce);
			otherRb2d.velocity = new Vector2(0, 0);
			other.SendMessageUpwards("OnDamaged", msg, SendMessageOptions.DontRequireReceiver);
			Debug.Log("Damage target");
		}
	}
}
