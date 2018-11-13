using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour {

	public float damage = 10f;
	public float repelForce = 10f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log("Trigger eneter");
		if (!other.isTrigger && other.CompareTag("Enemy")) {
			other.SendMessageUpwards("OnDamaged", damage, SendMessageOptions.DontRequireReceiver);
			Debug.Log("Damage target");

			// repel enemy from this collider
			Rigidbody2D otherRb2d = other.GetComponent<Rigidbody2D>();
			Vector2 direction = otherRb2d.position - new Vector2 (transform.position.x, transform.position.y);
			direction.Normalize();
			otherRb2d.velocity = new Vector2(0f, 0f);
			otherRb2d.AddForce(direction * repelForce, ForceMode2D.Impulse);
		}
	}
}
